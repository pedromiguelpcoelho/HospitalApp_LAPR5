import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import DeleteOperationRequestForm from '../../../Components/OperationRequests/DeleteOperationRequestForm';

describe('DeleteOperationRequestForm', () => {
    const mockOnDelete = jest.fn();
    const operationRequests = [
        { id: '1', patientName: 'John Doe', operationTypeName: 'Surgery', doctorName: 'Dr. Smith', priority: 'Elective', suggestedDeadline: '2023-12-31T12:00', requestDate: '2023-01-01T10:00' },
        { id: '2', patientName: 'Jane Smith', operationTypeName: 'Checkup', doctorName: 'Dr. Brown', priority: 'Urgent', suggestedDeadline: '2024-01-01T09:00', requestDate: '2023-02-01T11:00' }
    ];

    beforeEach(() => {
        mockOnDelete.mockClear();
    });

    test('renders the form with all fields', () => {
        render(
            <DeleteOperationRequestForm
                operationRequests={operationRequests}
                onDelete={mockOnDelete}
            />
        );

        expect(screen.getByText('Select Operation Request ID:')).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /delete/i })).toBeInTheDocument();
    });

    test('displays the selected request details', () => {
        render(
            <DeleteOperationRequestForm
                operationRequests={operationRequests}
                onDelete={mockOnDelete}
            />
        );

        fireEvent.change(screen.getByRole('combobox'), { target: { value: '1' } });

        const detailsContainer = screen.getByText('Operation Request Details').parentElement;

        expect(detailsContainer).toHaveTextContent('ID: 1');
        expect(detailsContainer).toHaveTextContent('Patient Name: John Doe');
        expect(detailsContainer).toHaveTextContent('Operation Type: Surgery');
        expect(detailsContainer).toHaveTextContent('Doctor Name: Dr. Smith');
        expect(detailsContainer).toHaveTextContent('Priority: Elective');
        expect(detailsContainer).toHaveTextContent('Expected Due Date: 12/31/2023, 12:00:00 PM');
        expect(detailsContainer).toHaveTextContent('Request Date: 1/1/2023, 10:00:00 AM');
    });

    test('calls onDelete with the correct ID when the form is submitted', () => {
        render(
            <DeleteOperationRequestForm
                operationRequests={operationRequests}
                onDelete={mockOnDelete}
            />
        );

        fireEvent.change(screen.getByRole('combobox'), { target: { value: '1' } });
        fireEvent.click(screen.getByRole('button', { name: /delete/i }));

        expect(mockOnDelete).toHaveBeenCalledWith('1');
    });
});
