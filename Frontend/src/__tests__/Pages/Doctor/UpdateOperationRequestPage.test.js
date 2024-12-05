import React from 'react';
import { render, screen, waitFor, fireEvent, act } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import UpdateOperationRequestPage from '../../../Pages/Doctor/UpdateOperationRequestPage';
import fetchApi from '../../../Services/fetchApi';

jest.mock('../../../Services/fetchApi');

describe('UpdateOperationRequestPage', () => {
    beforeEach(() => {
        fetchApi.mockClear();
    });

    afterEach(() => {
        jest.clearAllTimers();
        jest.resetAllMocks();
    });

    test('renders loading state initially', async () => {
        // Add a delay to simulate loading state
        fetchApi.mockImplementationOnce(() =>
            new Promise((resolve) => setTimeout(() => resolve({ operationRequests: [] }), 100))
        );

        // Render the component
        await act(async () => {
            render(<UpdateOperationRequestPage />);
        });

        // Assert loading state appears first
        expect(screen.getByText('Loading...')).toBeInTheDocument();

        // Wait for loading to complete
        await waitFor(() => {
            expect(screen.queryByText('Loading...')).not.toBeInTheDocument();
        });

        // Assert the page content appears
        expect(screen.getByText('Update Operation Request')).toBeInTheDocument();
    });

    test('renders error message on fetch failure', async () => {
        fetchApi.mockRejectedValueOnce(new Error('Failed to fetch operation requests'));

        await act(async () => {
            render(<UpdateOperationRequestPage />);
        });

        // Assert error message is displayed
        await waitFor(() => {
            expect(screen.getByText('Failed to fetch operation requests')).toBeInTheDocument();
        });
    });

    test('renders operation requests and form on fetch success', async () => {
        const operationRequests = [
            { id: '1', patientName: 'John Doe', priority: 'Elective', suggestedDeadline: '2023-12-31T12:00' },
            { id: '2', patientName: 'Jane Smith', priority: 'Urgent', suggestedDeadline: '2024-01-01T09:00' }
        ];
        fetchApi.mockResolvedValueOnce({ operationRequests });

        await act(async () => {
            render(<UpdateOperationRequestPage />);
        });

        // Wait for the effect to complete
        await waitFor(() => {
            expect(screen.getByText('Update Operation Request')).toBeInTheDocument();
            expect(screen.getByText('Select Operation Request ID:')).toBeInTheDocument();
        });
    });

    test('displays success message on update success', async () => {
        const operationRequests = [
            { id: '1', patientName: 'John Doe', priority: 'Elective', suggestedDeadline: '2023-12-31T12:00' }
        ];
        fetchApi.mockResolvedValueOnce({ operationRequests });
        fetchApi.mockResolvedValueOnce({}); // Mock successful update

        await act(async () => {
            render(<UpdateOperationRequestPage />);
        });

        // Interact with the form
        await waitFor(() => {
            fireEvent.change(screen.getByRole('combobox'), { target: { value: '1' } });
            fireEvent.change(screen.getByDisplayValue('Elective'), { target: { value: 'Urgent' } });
            fireEvent.click(screen.getByRole('button', { name: /update/i }));
        });

        // Assert success message
        await waitFor(() => {
            expect(screen.getByText('Operation request updated successfully!')).toBeInTheDocument();
        });
    });

    test('displays error message on update failure', async () => {
        const operationRequests = [
            { id: '1', patientName: 'John Doe', priority: 'Elective', suggestedDeadline: '2023-12-31T12:00' }
        ];
        fetchApi.mockResolvedValueOnce({ operationRequests });
        fetchApi.mockRejectedValueOnce(new Error('Failed to update operation request')); // Mock failed update

        await act(async () => {
            render(<UpdateOperationRequestPage />);
        });

        // Interact with the form
        await waitFor(() => {
            fireEvent.change(screen.getByRole('combobox'), { target: { value: '1' } });
            fireEvent.change(screen.getByDisplayValue('Elective'), { target: { value: 'Urgent' } });
            fireEvent.click(screen.getByRole('button', { name: /update/i }));
        });

        // Assert error message
        await waitFor(() => {
            expect(screen.getByText('Failed to update operation request')).toBeInTheDocument();
        });
    });
});
