import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import OperationRequestForm from '../../../Components/OperationRequests/AddOperationRequestForm';

describe('OperationRequestForm', () => {
    const mockOnCreateRequest = jest.fn();
    const patients = [{ id: 1, fullName: 'John Doe' }];
    const operationTypes = [{ id: 1, name: 'Surgery' }];
    const doctors = [{ id: 1, name: 'Dr. Smith' }];

    beforeEach(() => {
        mockOnCreateRequest.mockClear();
    });

    test('renders the form with all fields', () => {
        render(
            <OperationRequestForm
                onCreateRequest={mockOnCreateRequest}
                patients={patients}
                operationTypes={operationTypes}
                doctors={doctors}
            />
        );

        expect(screen.getByText('Add Operation Request')).toBeInTheDocument();
        expect(screen.getByText('Select Patient')).toBeInTheDocument();
        expect(screen.getByText('Select Operation Type')).toBeInTheDocument();
        expect(screen.getByText('Select Doctor')).toBeInTheDocument();
        expect(screen.getByText('Elective')).toBeInTheDocument();
        expect(screen.getByText('Urgent')).toBeInTheDocument();
        expect(screen.getByText('Emergency')).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /create/i })).toBeInTheDocument();
    });

    test('displays error message when form is submitted with missing fields', () => {
        render(
            <OperationRequestForm
                onCreateRequest={mockOnCreateRequest}
                patients={patients}
                operationTypes={operationTypes}
                doctors={doctors}
            />
        );

        fireEvent.click(screen.getByRole('button', { name: /create/i }));

        expect(screen.getByText('All fields are required.')).toBeInTheDocument();
        expect(mockOnCreateRequest).not.toHaveBeenCalled();
    });

    test('submits the form with correct data', () => {
        render(
            <OperationRequestForm
                onCreateRequest={mockOnCreateRequest}
                patients={patients}
                operationTypes={operationTypes}
                doctors={doctors}
            />
        );

        fireEvent.change(screen.getAllByRole('combobox')[0], { target: { value: '1' } });
        fireEvent.change(screen.getAllByRole('combobox')[1], { target: { value: '1' } });
        fireEvent.change(screen.getAllByRole('combobox')[2], { target: { value: '1' } });
        fireEvent.change(screen.getAllByRole('combobox')[3], { target: { value: 'Elective' } });
        const dateTimeInput = screen.getByDisplayValue(''); // Initially empty
        fireEvent.change(dateTimeInput, { target: { value: '2023-12-31T12:00' } });
        fireEvent.click(screen.getByRole('button', { name: /create/i }));

        expect(mockOnCreateRequest).toHaveBeenCalledWith({
            patientId: '1',
            operationTypeId: '1',
            doctorId: '1',
            priority: 'Elective',
            suggestedDeadline: new Date('2023-12-31T12:00')
        });
    });
});