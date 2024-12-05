import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import ListStaffPage from '../../../../Pages/Admin/StaffProfile/ListStaffPage';
import fetchApi from '../../../../Services/fetchApi';

jest.mock('../../../../Services/fetchApi');

describe('ListStaffPage', () => {
    const mockProfiles = [
        { id: '1', name: 'John Doe', email: 'john.doe@example.com', specialization: 'Orthopaedist',role: 'Doctor'  },
        { id: '2', name: 'Jane Smith', email: 'jane.smith@example.com', specialization: 'Orthopaedist',role:'Doctor' },
    ];

    beforeEach(() => {
        fetchApi.mockResolvedValue(mockProfiles);
    });

    afterEach(() => {
        jest.clearAllMocks();
    });

    

    /*test('filters profiles based on search input', async () => {
        render(<ListStaffPage />);

        // Wait for profiles to load
        await waitFor(() => {
            expect(fetchApi).toHaveBeenCalled();
        });

        // Simulate filter selection and input
        fireEvent.change(screen.getByRole('combobox'), { target: { value: 'role' } });
        fireEvent.change(screen.getByText(/role/i), { target: { value: 'Doctor' } });

        // Check if only the filtered profile is displayed
        await waitFor(() => {
            expect(screen.getByText('John Doe')).toBeInTheDocument();
            expect(screen.queryByText('Jane Smith')).not.toBeInTheDocument();
        });
    });*/


    test('handles API errors gracefully', async () => {
        fetchApi.mockRejectedValueOnce(new Error('Failed to fetch'));

        render(<ListStaffPage />);

        // Wait for error message
        await waitFor(() => {
            expect(screen.getByText(/failed to fetch/i)).toBeInTheDocument();
        });
    });
});
