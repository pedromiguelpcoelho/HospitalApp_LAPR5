import React from 'react';
import { render, screen, fireEvent, waitFor, act } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import DeactivateOperationTypesPage from '../../../../Pages/Admin/OperationType/DeactivateOperationTypesPage';
import fetchApi from '../../../../Services/fetchApi';

jest.mock('../../../../Services/fetchApi');

describe('DeactivateOperationTypesPage Tests', () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    test('shows error message when API call fails', async () => {
        // Mock da resposta para a busca de operation types
        fetchApi.mockResolvedValueOnce([
            { id: '1', name: 'Operation Type 1', requiredStaff: ['1'], estimatedDuration: 60, isActive: true }
        ]);

        // Mock da resposta para a busca de staff members
        fetchApi.mockResolvedValueOnce([
            { id: '1', name: 'John Doe', role: 'Doctor' }
        ]);

        // Mock da falha na desativação do operation type
        fetchApi.mockRejectedValueOnce(new Error('Failed to deactivate operation type'));

        await act(async () => {
            render(
                <MemoryRouter>
                    <DeactivateOperationTypesPage />
                </MemoryRouter>
            );
        });

        const operationType = screen.getByText('Operation Type 1');
        fireEvent.click(operationType);

        const deactivateButton = screen.getByText('Deactivate');
        fireEvent.click(deactivateButton);

        // Aguarda e verifica a mensagem de erro
        await waitFor(() => {
            expect(screen.getByText('Error: Failed to deactivate operation type')).toBeInTheDocument();
        });
    });
});