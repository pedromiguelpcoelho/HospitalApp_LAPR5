import React from 'react';
import { render, screen, fireEvent, waitFor, act } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import AddOperationRequestPage from '../../../Pages/Doctor/AddOperationRequestPage';
import fetchApi from '../../../Services/fetchApi';

jest.mock('../../../Services/fetchApi');

describe('AddOperationRequestPage', () => {
    const patients = [{ id: 1, fullName: 'John Doe' }];
    const operationTypes = [{ id: 1, name: 'Surgery' }];
    const doctors = [{ id: 1, name: 'Dr. Smith' }];

    beforeEach(() => {
        fetchApi.mockClear();
    });

    test('renders the form with all fields', async () => {
        fetchApi.mockResolvedValueOnce(patients)
            .mockResolvedValueOnce(operationTypes)
            .mockResolvedValueOnce(doctors);

        await act(async () => {
            render(<AddOperationRequestPage />);
        });

        await waitFor(() => {
            expect(screen.getByText('Add Operation Request')).toBeInTheDocument();
            expect(screen.getByText('Select Patient')).toBeInTheDocument();
            expect(screen.getByText('Select Operation Type')).toBeInTheDocument();
            expect(screen.getByText('Select Doctor')).toBeInTheDocument();
            expect(screen.getByText('Elective')).toBeInTheDocument();
            expect(screen.getByText('Urgent')).toBeInTheDocument();
            expect(screen.getByText('Emergency')).toBeInTheDocument();
            expect(screen.getByRole('button', { name: /create/i })).toBeInTheDocument();
        });
    });

    test('displays error message when form is submitted with missing fields', async () => {
        fetchApi.mockResolvedValueOnce(patients)
            .mockResolvedValueOnce(operationTypes)
            .mockResolvedValueOnce(doctors);

        await act(async () => {
            render(<AddOperationRequestPage />);
        });

        // Submit the form without filling any fields
        fireEvent.click(screen.getByRole('button', { name: /create/i }));

        // Wait for validation error message
        await waitFor(() => {
            expect(screen.getByText('All fields are required.')).toBeInTheDocument();
        });
    });

    test('submits the form with correct data', async () => {
        fetchApi.mockResolvedValueOnce(patients)
            .mockResolvedValueOnce(operationTypes)
            .mockResolvedValueOnce(doctors)
            .mockResolvedValueOnce({ status: 200 }); // Mock successful response

        await act(async () => {
            render(<AddOperationRequestPage />);
        });

        // Fill the form fields
        fireEvent.change(screen.getAllByRole('combobox')[0], { target: { value: '1' } }); // Patient
        fireEvent.change(screen.getAllByRole('combobox')[1], { target: { value: '1' } }); // Operation Type
        fireEvent.change(screen.getAllByRole('combobox')[2], { target: { value: '1' } }); // Doctor
        fireEvent.change(screen.getAllByRole('combobox')[3], { target: { value: 'Elective' } }); // Priority
        fireEvent.change(screen.getByDisplayValue(''), { target: { value: '2023-12-31T12:00' } }); // Suggested Deadline

        // Submit the form
        fireEvent.click(screen.getByRole('button', { name: /create/i }));

        // Wait for success message
        await waitFor(() => {
            expect(screen.getByText('Operation request created successfully!')).toBeInTheDocument();
        });
    });

    test('displays error message when API call fails', async () => {
        fetchApi.mockResolvedValueOnce(patients)
            .mockResolvedValueOnce(operationTypes)
            .mockResolvedValueOnce(doctors)
            .mockRejectedValueOnce(new Error('Failed to create operation request')); // Mock API failure

        await act(async () => {
            render(<AddOperationRequestPage />);
        });

        // Fill the form fields
        fireEvent.change(screen.getAllByRole('combobox')[0], { target: { value: '1' } }); // Patient
        fireEvent.change(screen.getAllByRole('combobox')[1], { target: { value: '1' } }); // Operation Type
        fireEvent.change(screen.getAllByRole('combobox')[2], { target: { value: '1' } }); // Doctor
        fireEvent.change(screen.getAllByRole('combobox')[3], { target: { value: 'Elective' } }); // Priority
        fireEvent.change(screen.getByDisplayValue(''), { target: { value: '2023-12-31T12:00' } }); // Suggested Deadline

        // Submit the form
        fireEvent.click(screen.getByRole('button', { name: /create/i }));

        // Wait for error message
        await waitFor(() => {
            expect(screen.getByText('Failed to create operation request')).toBeInTheDocument();
        });
    });

    test('displays loading state during form submission', async () => {
        fetchApi.mockResolvedValueOnce(patients)
            .mockResolvedValueOnce(operationTypes)
            .mockResolvedValueOnce(doctors)
            .mockResolvedValueOnce({ status: 200 }); // Mock successful response

        await act(async () => {
            render(<AddOperationRequestPage />);
        });

        // Fill the form fields
        fireEvent.change(screen.getAllByRole('combobox')[0], { target: { value: '1' } }); // Patient
        fireEvent.change(screen.getAllByRole('combobox')[1], { target: { value: '1' } }); // Operation Type
        fireEvent.change(screen.getAllByRole('combobox')[2], { target: { value: '1' } }); // Doctor
        fireEvent.change(screen.getAllByRole('combobox')[3], { target: { value: 'Elective' } }); // Priority
        fireEvent.change(screen.getByDisplayValue(''), { target: { value: '2023-12-31T12:00' } }); // Suggested Deadline

        // Trigger form submission
        fireEvent.click(screen.getByRole('button', { name: /create/i }));

        // Wait for loading message
        await waitFor(() => {
            expect(screen.getByText('Creating...')).toBeInTheDocument();
        });
    });
});
