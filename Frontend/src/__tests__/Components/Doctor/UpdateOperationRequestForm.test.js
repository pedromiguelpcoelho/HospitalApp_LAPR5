import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import UpdateOperationRequestForm from '../../../Components/OperationRequests/UpdateOperationRequestForm';

describe('UpdateOperationRequestForm', () => {
    const mockOnUpdateRequest = jest.fn();
    const operationRequests = [
        { id: '1', patientName: 'John Doe', priority: 'Elective', suggestedDeadline: '2023-12-31T12:00' },
        { id: '2', patientName: 'Jane Smith', priority: 'Urgent', suggestedDeadline: '2024-01-01T09:00' }
    ];

    beforeEach(() => {
        mockOnUpdateRequest.mockClear();
    });

    test('renders the form with all fields', () => {
        render(
            <UpdateOperationRequestForm
                operationRequests={operationRequests}
                onUpdateRequest={mockOnUpdateRequest}
            />
        );

        expect(screen.getByText('Select Operation Request ID:')).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /update/i })).toBeInTheDocument();
    });

    test('displays the selected request details', async () => {
        render(
            <UpdateOperationRequestForm
                operationRequests={operationRequests}
                onUpdateRequest={mockOnUpdateRequest}
            />
        );

        fireEvent.change(screen.getByRole('combobox'), { target: { value: '1' } });

        await waitFor(() => {
            expect(screen.getByDisplayValue('Elective')).toBeInTheDocument();
            expect(screen.getByDisplayValue('2023-12-31T12:00')).toBeInTheDocument();
        });
    });

    test('submits the form with updated data', async () => {
        render(
            <UpdateOperationRequestForm
                operationRequests={operationRequests}
                onUpdateRequest={mockOnUpdateRequest}
            />
        );

        fireEvent.change(screen.getAllByRole('combobox')[0], { target: { value: '1' } });

        await waitFor(() => {
            fireEvent.change(screen.getAllByRole('combobox')[1], { target: { value: 'Urgent' } });
        });

        fireEvent.click(screen.getByRole('button', { name: /update/i }));

        await waitFor(() => {
            expect(mockOnUpdateRequest).toHaveBeenCalledWith({
                id: '1',
                priority: 'Urgent',
                suggestedDeadline: new Date('2023-12-31T12:00')
            });
        });
    });

    test('submits the form without updating any fields', async () => {
        render(
            <UpdateOperationRequestForm
                operationRequests={operationRequests}
                onUpdateRequest={mockOnUpdateRequest}
            />
        );

        fireEvent.change(screen.getAllByRole('combobox')[0], { target: { value: '1' } });

        fireEvent.click(screen.getByRole('button', { name: /update/i }));

        await waitFor(() => {
            expect(mockOnUpdateRequest).toHaveBeenCalledWith({
                id: '1',
                priority: 'Elective',
                suggestedDeadline: new Date('2023-12-31T12:00')
            });
        });
    }); 
});