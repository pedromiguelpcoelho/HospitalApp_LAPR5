import React from 'react';
import { render, screen, fireEvent, waitFor, act } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import SearchOperationRequestPage from '../../../Pages/Doctor/SearchOperationRequestPage';
import fetchApi from '../../../Services/fetchApi';

jest.mock('../../../Services/fetchApi');

describe('SearchOperationRequestPage', () => {
    const operationRequests = [
        { id: '1', patientName: 'John Doe', operationTypeName: 'Surgery', doctorName: 'Dr. Smith', priority: 'Urgent', suggestedDeadline: '2023-12-31T12:00', requestDate: '2023-01-01T10:00' },
        { id: '2', patientName: 'Jane Smith', operationTypeName: 'Checkup', doctorName: 'Dr. Brown', priority: 'Elective', suggestedDeadline: '2024-01-01T09:00', requestDate: '2023-01-02T11:00' }
    ];

    beforeEach(() => {
        fetchApi.mockClear();
    });

    test('renders the page with search form and no initial results', async () => {
        fetchApi.mockResolvedValueOnce({ operationRequests: [] });

        await act(async () => {
            render(<SearchOperationRequestPage />);
        });

        expect(screen.getByText('Search Operation Requests')).toBeInTheDocument();
        expect(screen.getByText('No operation requests found.')).toBeInTheDocument();
    });

    test('displays search results when search is successful', async () => {
        fetchApi.mockResolvedValueOnce({ operationRequests });

        await act(async () => {
            render(<SearchOperationRequestPage />);
        });

        fireEvent.change(screen.getAllByPlaceholderText('Insert...')[0], { target: { value: 'John Doe' } });
        fireEvent.click(screen.getByRole('button', { name: /search/i }));

        await waitFor(() => {
            expect(screen.getByText('Patient: John Doe')).toBeInTheDocument();
            expect(screen.getByText('Patient: Jane Smith')).toBeInTheDocument();
        });
    });

    test('displays error message when search fails', async () => {
        fetchApi.mockRejectedValueOnce(new Error('Failed to fetch operation requests'));

        await act(async () => {
            render(<SearchOperationRequestPage />);
        });

        fireEvent.click(screen.getByRole('button', { name: /search/i }));

        await waitFor(() => {
            expect(screen.getByText('Failed to fetch operation requests')).toBeInTheDocument();
        });
    });

    test('toggles details of an operation request', async () => {
        fetchApi.mockResolvedValueOnce({ operationRequests });

        let container;
        await act(async () => {
            const renderResult = render(<SearchOperationRequestPage />);
            container = renderResult.container;
        });

        // Click to expand details of John Doe
        fireEvent.click(screen.getByText('Patient: John Doe'));

        await waitFor(() => {
            // Select the <p> element containing the "Operation Type" text
            const operationTypeElement = container.querySelector(
                '.search-results-item div p:nth-child(3)'
            );
            expect(operationTypeElement).toHaveTextContent('Operation Type: Surgery');
        });

        // Click again to collapse details of John Doe
        fireEvent.click(screen.getByText('Patient: John Doe'));

        await waitFor(() => {
            const operationTypeElement = container.querySelector(
                '.search-results-item div p:nth-child(3)'
            );
            expect(operationTypeElement).not.toBeInTheDocument();
        });
    });
});
