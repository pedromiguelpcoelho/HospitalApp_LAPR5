import React from 'react';
import { render, screen, fireEvent, waitFor, act } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import AddOperationTypesPage from '../../../../Pages/Admin/OperationType/AddOperationTypesPage';
import fetchApi from '../../../../Services/fetchApi';

jest.mock('../../../../Services/fetchApi');

describe('AddOperationTypePage Tests', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  test('renders AddOperationTypePage and submits data successfully', async () => {
    // Mock para buscar membros do staff
    fetchApi.mockResolvedValueOnce([
      { id: '1', name: 'John Doe', role: 'Doctor' },
      { id: '2', name: 'Jane Smith', role: 'Nurse' },
    ]);

    // Mock para criar um Operation Type
    fetchApi.mockResolvedValueOnce({ message: 'Operation type created successfully' });

    window.alert = jest.fn();

    await act(async () => {
      render(
        <MemoryRouter>
          <AddOperationTypesPage />
        </MemoryRouter>
      );
    });

    // Preencher os campos
    const nameInput = screen.getByLabelText('Name:');
    const estimatedDurationInput = screen.getByLabelText('Estimated Duration (min):');
    const submitButton = screen.getByRole('button', { name: /add operation type/i });

    fireEvent.change(nameInput, { target: { value: 'Operation Type 1' } });
    fireEvent.change(estimatedDurationInput, { target: { value: '60' } });

    // Simular seleção de staff
    const staffCheckbox = screen.getByLabelText('John Doe');
    fireEvent.click(staffCheckbox);

    fireEvent.click(submitButton);

    // Validar chamadas ao fetchApi
    await waitFor(() => {
      expect(fetchApi).toHaveBeenCalledWith('/staff/all');
    });

    await waitFor(() => {
      expect(fetchApi).toHaveBeenCalledWith('/operationTypes', {
        method: 'POST',
        body: JSON.stringify({
          name: 'Operation Type 1',
          requiredStaff: ['1'],
          estimatedDuration: '60',
        }),
      });
    });

    expect(window.alert).toHaveBeenCalledWith('Operation type created successfully!');
  });

  test('shows loading indicator while creating operation type', async () => {
    fetchApi.mockResolvedValueOnce([
      { id: '1', name: 'John Doe', role: 'Doctor' },
      { id: '2', name: 'Jane Smith', role: 'Nurse' },
    ]);
    fetchApi.mockResolvedValueOnce({}); // Simulação de criação de tipo de operação

    await act(async () => {
      render(
        <MemoryRouter>
          <AddOperationTypesPage />
        </MemoryRouter>
      );
    });

    const nameInput = screen.getByLabelText('Name:');
    const estimatedDurationInput = screen.getByLabelText('Estimated Duration (min):');
    const submitButton = screen.getByRole('button', { name: /add operation type/i });

    fireEvent.change(nameInput, { target: { value: 'Operation Type 1' } });
    fireEvent.change(estimatedDurationInput, { target: { value: '60' } });

    const staffCheckbox = screen.getByLabelText('John Doe');
    fireEvent.click(staffCheckbox);

    fireEvent.click(submitButton);

    // Verificar que o indicador de carregamento aparece
    await waitFor(() => {
      expect(screen.getByText('Creating...')).toBeInTheDocument();
    });

    await waitFor(() => {
      expect(fetchApi).toHaveBeenCalledWith('/operationTypes', {
        method: 'POST',
        body: JSON.stringify({
          name: 'Operation Type 1',
          requiredStaff: ['1'],
          estimatedDuration: '60',
        }),
      });
    });
  });

  test('shows error message when API call fails', async () => {
    fetchApi.mockResolvedValueOnce([
      { id: '1', name: 'John Doe', role: 'Doctor' },
      { id: '2', name: 'Jane Smith', role: 'Nurse' },
    ]);
    fetchApi.mockRejectedValueOnce(new Error('Failed to create operation type'));

    await act(async () => {
      render(
        <MemoryRouter>
          <AddOperationTypesPage />
        </MemoryRouter>
      );
    });

    const nameInput = screen.getByLabelText('Name:');
    const estimatedDurationInput = screen.getByLabelText('Estimated Duration (min):');
    const submitButton = screen.getByRole('button', { name: /add operation type/i });

    fireEvent.change(nameInput, { target: { value: 'Operation Type 1' } });
    fireEvent.change(estimatedDurationInput, { target: { value: '60' } });

    const staffCheckbox = screen.getByLabelText('John Doe');
    fireEvent.click(staffCheckbox);

    fireEvent.click(submitButton);

    // Validar que a mensagem de erro é exibida
    await waitFor(() => {
      expect(fetchApi).toHaveBeenCalledWith('/operationTypes', {
        method: 'POST',
        body: JSON.stringify({
          name: 'Operation Type 1',
          requiredStaff: ['1'],
          estimatedDuration: '60',
        }),
      });
    });

    await waitFor(() => {
        expect(screen.getByText((content, element) =>
          content.includes('Failed to create operation type')
        )).toBeInTheDocument();
      });
      
  });
});
