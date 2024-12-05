import React from 'react';
import { render, screen, waitFor, fireEvent, act } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import DeleteOperationRequestPage from '../../../Pages/Doctor/DeleteOperationRequestPage';
import fetchApi from '../../../Services/fetchApi';

jest.mock('../../../Services/fetchApi'); // Mock do serviço de API

describe('DeleteOperationRequestPage', () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    test('renders the page and fetches operation requests', async () => {
        const mockOperationRequests = [
            { id: '1', patientName: 'John Doe' },
            { id: '2', patientName: 'Jane Smith' },
        ];

        fetchApi.mockResolvedValueOnce({ operationRequests: mockOperationRequests });

        await act(async () => {
            render(<DeleteOperationRequestPage />);
        });

        expect(screen.getByText(/delete operation request/i)).toBeInTheDocument();
        await waitFor(() => expect(fetchApi).toHaveBeenCalledWith('/operationrequests/searchByDoctor'));
        expect(screen.getByText('1 - John Doe')).toBeInTheDocument();
        expect(screen.getByText('2 - Jane Smith')).toBeInTheDocument();
    });

    test('deletes an operation request and updates the list', async () => {
        const mockOperationRequests = [
            { id: '1', patientName: 'John Doe' },
            { id: '2', patientName: 'Jane Smith' },
        ];

        fetchApi
            .mockResolvedValueOnce({ operationRequests: mockOperationRequests }) // Fetch inicial
            .mockResolvedValueOnce(); // Delete

        await act(async () => {
            render(<DeleteOperationRequestPage />);
        });

        await waitFor(() => expect(screen.getByText('1 - John Doe')).toBeInTheDocument());

        // Simula a seleção do ID no dropdown
        fireEvent.change(screen.getByRole('combobox'), { target: { value: '1' } });

        global.confirm = jest.fn(() => true); // Mock da confirmação
        fireEvent.click(screen.getByText(/delete/i, { selector: 'button' }));

        // Verifica se a URL contém o ID correto
        await waitFor(() =>
            expect(fetchApi).toHaveBeenCalledWith(
                '/operationrequests/delete/1',
                { method: 'DELETE' }
            )
        );

        await waitFor(() => expect(screen.getByText(/operation request deleted successfully/i)).toBeInTheDocument());
    });

    test('displays an error message when delete fails', async () => {
        const mockOperationRequests = [
            { id: '1', patientName: 'John Doe' },
        ];

        fetchApi
            .mockResolvedValueOnce({ operationRequests: mockOperationRequests }) // Fetch inicial
            .mockRejectedValueOnce(new Error('Delete failed')); // Delete

        await act(async () => {
            render(<DeleteOperationRequestPage />);
        });

        await waitFor(() => expect(screen.getByText('1 - John Doe')).toBeInTheDocument());

        // Simula a seleção do ID no dropdown
        fireEvent.change(screen.getByRole('combobox'), { target: { value: '1' } });

        global.confirm = jest.fn(() => true); // Mock da confirmação
        fireEvent.click(screen.getByText(/delete/i, { selector: 'button' }));

        // Verifica se a URL contém o ID correto
        await waitFor(() =>
            expect(fetchApi).toHaveBeenCalledWith(
                '/operationrequests/delete/1',
                { method: 'DELETE' }
            )
        );

        // Verifica a exibição da mensagem de erro
        await waitFor(() => expect(screen.getByText(/delete failed/i)).toBeInTheDocument());
    });
});
