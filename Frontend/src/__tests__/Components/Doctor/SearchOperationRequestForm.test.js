import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import SearchOperationRequestForm from '../../../Components/OperationRequests/SearchOperationRequestForm';

describe('SearchOperationRequestForm', () => {
    const mockOnSearch = jest.fn();

    beforeEach(() => {
        mockOnSearch.mockClear();
    });

    test('renders the form with all fields', () => {
        render(<SearchOperationRequestForm onSearch={mockOnSearch} />);

        expect(screen.getByText("Search Operation Requests")).toBeInTheDocument();
        expect(screen.getAllByPlaceholderText("Insert...").length).toBe(2);
        expect(screen.getByPlaceholderText("Expected Due Date")).toBeInTheDocument();
        expect(screen.getByPlaceholderText("Request Date")).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /search/i })).toBeInTheDocument();
    });

    test('calls onSearch with the correct data when the form is submitted', () => {
        render(<SearchOperationRequestForm onSearch={mockOnSearch} />);

        fireEvent.change(screen.getAllByPlaceholderText("Insert...")[0], { target: { value: 'John Doe' } });
        fireEvent.change(screen.getAllByPlaceholderText("Insert...")[1], { target: { value: 'Surgery' } });
        fireEvent.change(screen.getByRole('combobox'), { target: { value: 'Urgent' } });
        fireEvent.change(screen.getByPlaceholderText('Expected Due Date'), { target: { value: '2023-12-31T12:00' } });
        fireEvent.change(screen.getByPlaceholderText('Request Date'), { target: { value: '2023-01-01T10:00' } });

        fireEvent.click(screen.getByRole('button', { name: /search/i }));

        expect(mockOnSearch).toHaveBeenCalledWith({
            priority: 'Urgent',
            operationTypeName: 'Surgery',
            patientName: 'John Doe',
            expectedDueDate: '2023-12-31T12:00',
            requestDate: '2023-01-01T10:00'
        });
    });

    test('calls onSearch with empty fields when the form is submitted without input', () => {
        render(<SearchOperationRequestForm onSearch={mockOnSearch} />);

        fireEvent.click(screen.getByRole('button', { name: /search/i }));

        expect(mockOnSearch).toHaveBeenCalledWith({
            priority: '',
            operationTypeName: '',
            patientName: '',
            expectedDueDate: '',
            requestDate: ''
        });
    });
});