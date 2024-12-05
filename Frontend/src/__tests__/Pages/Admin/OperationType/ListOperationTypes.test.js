import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import ListOperationTypesPage from '../../../../Pages/Admin/OperationType/ListOperationTypesPage';

import fetchApi from '../../../../Services/fetchApi';

jest.mock('../../../../Services/fetchApi');

describe('ListOperationTypesPage', () => {
    afterEach(() => {
        jest.clearAllMocks();
    });

    test('displays loading message initially', () => {
        render(<ListOperationTypesPage />);
        expect(screen.getByText('Loading operation types...')).toBeInTheDocument();
    });

    test('displays error message if fetching data fails', async () => {
        fetchApi.mockImplementation(() => {
            throw new Error('Fetch failed');
        });

        render(<ListOperationTypesPage />);

    });

    test('renders operation types and toggles active/inactive filter', async () => {
        fetchApi.mockImplementation((url) => {
            if (url === '/operationTypes/all') {
                return Promise.resolve([
                    { id: 1, name: 'Operation A', isActive: true },
                    { id: 2, name: 'Operation B', isActive: false },
                ]);
            }
            if (url === '/staff/all') {
                return Promise.resolve([]);
            }
        });

        render(<ListOperationTypesPage />);

        await waitFor(() => {
            expect(screen.getByText('List of Operation Types')).toBeInTheDocument();
            expect(screen.getByText('Operation A')).toBeInTheDocument();
        });

        const toggleButton = screen.getByText('Show Inactive');
        fireEvent.click(toggleButton);

        await waitFor(() => {
            expect(screen.getByText('Show Active')).toBeInTheDocument();
            expect(screen.getByText('Operation B')).toBeInTheDocument();
        });
    });

    test('opens details page when operation type is selected', async () => {
        fetchApi.mockImplementation((url) => {
            if (url === '/operationTypes/all') {
                return Promise.resolve([
                    { id: 1, name: 'Operation A', isActive: true },
                ]);
            }
            if (url === '/staff/all') {
                return Promise.resolve([
                    { id: 1, name: 'Staff A', role: 'Doctor' },
                ]);
            }
        });

        render(<ListOperationTypesPage />);

        await waitFor(() => {
            expect(screen.getByText('Operation A')).toBeInTheDocument();
        });

        const operationType = screen.getByText('Operation A');
        fireEvent.click(operationType);

        await waitFor(() => {
            expect(screen.getByText('Back to List')).toBeInTheDocument();
            expect(screen.getByText(/Operation A/)).toBeInTheDocument(); // Details
        });

        const backButton = screen.getByText('Back to List');
        fireEvent.click(backButton);

        await waitFor(() => {
            expect(screen.getByText('Operation A')).toBeInTheDocument();
        });
    });
});
