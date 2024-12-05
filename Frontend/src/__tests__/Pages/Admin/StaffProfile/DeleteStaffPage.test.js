import React from 'react';
import { render, screen, waitFor, fireEvent, act } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import DeleteStaffPage from '../../../../Pages/Admin/StaffProfile/DeleteStaffPage';
import fetchApi from '../../../../Services/fetchApi';

jest.mock('../../../../Services/fetchApi');

describe('DeleteStaffPage', () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    test('renders the page and fetches staff list', async () => {
        const mockStaffList = [
            { id: '1', name: 'John Doe', email: 'john.doe@example.com', specialization: 'Orthopaedist' },
            { id: '2', name: 'Jane Smith', email: 'jane.smith@example.com', specialization: 'Orthopaedist' },
        ];

        fetchApi.mockResolvedValueOnce(mockStaffList);

        await act(async () => {
            render(<DeleteStaffPage />);
        });

        expect(screen.getByText(/deactivate staff/i)).toBeInTheDocument();

       
        await waitFor(() => expect(fetchApi).toHaveBeenCalledWith('/staff/all'));

        
        expect(screen.getByText('1 - John Doe')).toBeInTheDocument();
        expect(screen.getByText('2 - Jane Smith')).toBeInTheDocument();
    });

    test('deactivates a staff member and updates the list', async () => {
        const mockStaffList = [
            { id: '1', name: 'John Doe', email: 'john.doe@example.com', specialization: 'Cardiologist' },
            { id: '2', name: 'Jane Smith', email: 'jane.smith@example.com', specialization: 'Orthopaedist' },
        ];

        fetchApi
            .mockResolvedValueOnce(mockStaffList) 
            .mockResolvedValueOnce(); 

        await act(async () => {
            render(<DeleteStaffPage />);
        });

        await waitFor(() => expect(screen.getByText('1 - John Doe')).toBeInTheDocument());

       
        fireEvent.change(screen.getByRole('combobox'), { target: { value: '1' } });

        global.confirm = jest.fn(() => true); 
        fireEvent.click(screen.getByText(/deactivate/i, { selector: 'button' }));

       
        await waitFor(() =>
            expect(fetchApi).toHaveBeenCalledWith('/staff/1', { method: 'DELETE' })
        );

        
        await waitFor(() => expect(screen.getByText(/staff successfully deactivated/i)).toBeInTheDocument());
    });

    test('displays an error message when deactivation fails', async () => {
        const mockStaffList = [
            { id: '1', name: 'John Doe', email: 'john.doe@example.com', specialization: 'Cardiologist' },
        ];

        fetchApi
            .mockResolvedValueOnce(mockStaffList) 
            .mockRejectedValueOnce(new Error('Deactivation failed')); 

        await act(async () => {
            render(<DeleteStaffPage />);
        });

        await waitFor(() => expect(screen.getByText('1 - John Doe')).toBeInTheDocument());

        
        fireEvent.change(screen.getByRole('combobox'), { target: { value: '1' } });

        global.confirm = jest.fn(() => true);
        fireEvent.click(screen.getByText(/deactivate/i, { selector: 'button' }));

        
        await waitFor(() =>
            expect(fetchApi).toHaveBeenCalledWith('/staff/1', { method: 'DELETE' })
        );

        
        await waitFor(() => expect(screen.getByText(/deactivation failed/i)).toBeInTheDocument());
    });
});
