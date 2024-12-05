import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import ConfirmDeletionOfPatientProfilePage from '../../../Pages/Patient/ConfirmDeletionOfPatientProfilePage';
import fetchApi from '../../../Services/fetchApi';

jest.mock('../../../Services/fetchApi');

describe('ConfirmDeletionOfPatientProfilePage', () => {
    beforeEach(() => {
        // Mock console.error to suppress error messages during tests
        jest.spyOn(console, 'error').mockImplementation(() => {});
        render(<ConfirmDeletionOfPatientProfilePage />);
    });

    afterEach(() => {
        // Restore console.error after each test
        console.error.mockRestore();
    });

    test('renders the form', () => {
        expect(screen.getByRole('button', { name: /Confirm Deletion/i })).toBeInTheDocument();
    });

    test('submits the form successfully', async () => {
        fetchApi.mockResolvedValueOnce({});

        fireEvent.change(screen.getByLabelText(/Confirmation Token/i), { target: { value: 'test-token' } });
        fireEvent.click(screen.getByRole('button', { name: /Confirm Deletion/i }));

        await waitFor(() => {
            expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/confirm-deletion?token=test-token', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        });

        expect(await screen.findByText(/Account deletion confirmed./i)).toBeInTheDocument();
    });

    test('shows error message on form submission failure', async () => {
        fetchApi.mockRejectedValueOnce(new Error('Request failed'));

        fireEvent.change(screen.getByLabelText(/Confirmation Token/i), { target: { value: 'test-token' } });
        fireEvent.click(screen.getByRole('button', { name: /Confirm Deletion/i }));

        await waitFor(() => {
            expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/confirm-deletion?token=test-token', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        });

        expect(await screen.findByText(/Failed to confirm account deletion./i)).toBeInTheDocument();
    });

    test('displays no message initially', () => {
        expect(screen.queryByText(/Account deletion confirmed./i)).not.toBeInTheDocument();
        expect(screen.queryByText(/Failed to confirm account deletion./i)).not.toBeInTheDocument();
    });

    test('handles multiple submissions correctly', async () => {
        fetchApi.mockResolvedValueOnce({});
        fetchApi.mockRejectedValueOnce(new Error('Request failed'));

        fireEvent.change(screen.getByLabelText(/Confirmation Token/i), { target: { value: 'test-token' } });
        fireEvent.click(screen.getByRole('button', { name: /Confirm Deletion/i }));

        await waitFor(() => {
            expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/confirm-deletion?token=test-token', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        });

        expect(await screen.findByText(/Account deletion confirmed./i)).toBeInTheDocument();

        fireEvent.change(screen.getByLabelText(/Confirmation Token/i), { target: { value: 'test-token-2' } });
        fireEvent.click(screen.getByRole('button', { name: /Confirm Deletion/i }));

        await waitFor(() => {
            expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/confirm-deletion?token=test-token-2', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        });

        expect(await screen.findByText(/Failed to confirm account deletion./i)).toBeInTheDocument();
    });
});