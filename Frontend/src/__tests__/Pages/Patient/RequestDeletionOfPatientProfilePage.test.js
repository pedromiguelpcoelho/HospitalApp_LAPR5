import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import RequestDeletionOfPatientProfilePage from '../../../Pages/Patient/RequestDeletionOfPatientProfilePage';
import fetchApi from '../../../Services/fetchApi';

jest.mock('../../../Services/fetchApi');

describe('RequestDeletionOfPatientProfilePage', () => {
    beforeEach(() => {
        // Mock console.error to suppress error messages during tests
        jest.spyOn(console, 'error').mockImplementation(() => {});
        render(<RequestDeletionOfPatientProfilePage />);
    });

    afterEach(() => {
        // Restore console.error after each test
        console.error.mockRestore();
    });

    test('renders the form', () => {
        expect(screen.getByRole('button', { name: /Request Deletion/i })).toBeInTheDocument();
    });

    test('submits the form successfully', async () => {
        fetchApi.mockResolvedValueOnce({});

        fireEvent.click(screen.getByRole('button', { name: /Request Deletion/i }));

        await waitFor(() => {
            expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/request-deletion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        });

        expect(await screen.findByText(/A confirmation email has been sent to your email address./i)).toBeInTheDocument();
    });

    test('shows error message on form submission failure', async () => {
        fetchApi.mockRejectedValueOnce(new Error('Request failed'));

        fireEvent.click(screen.getByRole('button', { name: /Request Deletion/i }));

        await waitFor(() => {
            expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/request-deletion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        });

        expect(await screen.findByText(/Failed to request account deletion./i)).toBeInTheDocument();
    });

    test('displays no message initially', () => {
        expect(screen.queryByText(/A confirmation email has been sent to your email address./i)).not.toBeInTheDocument();
        expect(screen.queryByText(/Failed to request account deletion./i)).not.toBeInTheDocument();
    });

    test('handles multiple submissions correctly', async () => {
        fetchApi.mockResolvedValueOnce({});
        fetchApi.mockResolvedValueOnce({});

        const button = screen.getByRole('button', { name: /Request Deletion/i });
        fireEvent.click(button);

        await waitFor(() => {
            expect(fetchApi).toHaveBeenCalledTimes(1);
        });

        fireEvent.click(button);

        await waitFor(() => {
            expect(fetchApi).toHaveBeenCalledTimes(2);
        });

        expect(await screen.findByText(/A confirmation email has been sent to your email address./i)).toBeInTheDocument();
    });
});