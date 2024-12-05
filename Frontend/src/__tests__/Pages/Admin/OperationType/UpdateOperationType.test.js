import React from 'react';
import { render, screen, fireEvent, waitFor, act } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import UpdateOperationTypePage from '../../../../Pages/Admin/OperationType/UpdateOperationTypesPage';
import fetchApi from '../../../../Services/fetchApi';

jest.mock('../../../../Services/fetchApi');

describe('UpdateOperationTypesPage Tests', () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    test('shows the update form when an operation type is selected', async () => {
        // Mock da resposta para a busca de operation types
        fetchApi.mockResolvedValueOnce([
            { id: '1', name: 'Operation Type 1', requiredStaff: ['1'], estimatedDuration: 60, isActive: true }
        ]);

        // Mock da resposta para a busca de staff members
        fetchApi.mockResolvedValueOnce([
            { id: '1', name: 'John Doe', role: 'Doctor' },
            { id: '2', name: 'Jane Smith', role: 'Nurse' }
        ]);

        await act(async () => {
            render(
                <MemoryRouter>
                    <UpdateOperationTypePage />
                </MemoryRouter>
            );
        });

        const operationType = screen.getByText('Operation Type 1');
        fireEvent.click(operationType);

        // Verifica se o formulário aparece
        expect(screen.getAllByText('Update Operation Type')).toHaveLength(2); // h1 e h2
        expect(screen.getByLabelText('Name:')).toHaveValue('Operation Type 1');
    });

    test('updates operation type successfully', async () => {
        // Mock da resposta para a busca de operation types
        fetchApi.mockResolvedValueOnce([
            { id: '1', name: 'Operation Type 1', requiredStaff: ['1'], estimatedDuration: 60, isActive: true }
        ]);
    
        // Mock da resposta para a busca de staff members
        fetchApi.mockResolvedValueOnce([
            { id: '1', name: 'John Doe', role: 'Doctor' },
            { id: '2', name: 'Jane Smith', role: 'Nurse' }
        ]);
    
        // Mock da resposta para a atualização do operation type
        fetchApi.mockResolvedValueOnce({ message: 'Operation type updated successfully' });
    
        // Mock window.alert
        jest.spyOn(window, 'alert').mockImplementation(() => {});
    
        await act(async () => {
            render(
                <MemoryRouter>
                    <UpdateOperationTypePage />
                </MemoryRouter>
            );
        });
    
        const operationType = screen.getByText('Operation Type 1');
        fireEvent.click(operationType);
    
        // Simula alteração no formulário
        const nameInput = screen.getByLabelText('Name:');
        fireEvent.change(nameInput, { target: { value: 'Updated Operation Type' } });
    
        const updateButton = screen.getByText('Update');
        fireEvent.click(updateButton);
    
        // Verifica se o feedback de sucesso aparece
        await waitFor(() => {
            expect(window.alert).toHaveBeenCalledWith('Operation type updated successfully!');
        });
    
        // Restore the original alert function
        window.alert.mockRestore();
    });
    

    test('shows error message when API call fails', async () => {
        // Mock da resposta para a busca de operation types
        fetchApi.mockResolvedValueOnce([
            { id: '1', name: 'Operation Type 1', requiredStaff: ['1'], estimatedDuration: 60, isActive: true }
        ]);

        // Mock da resposta para a busca de staff members
        fetchApi.mockResolvedValueOnce([
            { id: '1', name: 'John Doe', role: 'Doctor' },
            { id: '2', name: 'Jane Smith', role: 'Nurse' }
        ]);

        // Mock da falha na atualização do operation type
        fetchApi.mockRejectedValueOnce(new Error('Failed to update operation type'));

        await act(async () => {
            render(
                <MemoryRouter>
                    <UpdateOperationTypePage />
                </MemoryRouter>
            );
        });

        const operationType = screen.getByText('Operation Type 1');
        fireEvent.click(operationType);

        const updateButton = screen.getByText('Update');
        fireEvent.click(updateButton);

        // Verifica se o feedback de erro aparece
        await waitFor(() => {
            expect(screen.getByText((content) =>
                content.startsWith('Error: Failed to update operation type')
            )).toBeInTheDocument();
        });
    });
});