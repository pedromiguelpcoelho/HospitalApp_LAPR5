import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import UpdateMyOwnPatientProfilePage from '../../../Pages/Patient/UpdateMyOwnPatientProfilePage';
import fetchApi from '../../../Services/fetchApi';

jest.mock('../../../Services/fetchApi');

describe('UpdateMyOwnPatientProfilePage', () => {
    beforeEach(() => {
        // Mock window.alert and window.confirm
        window.alert = jest.fn();
        window.confirm = jest.fn().mockReturnValue(true);
        // Mock console.error to suppress error messages during tests
        jest.spyOn(console, 'error').mockImplementation(() => {});
        render(<UpdateMyOwnPatientProfilePage />);
    });

    afterEach(() => {
        // Restore console.error after each test
        console.error.mockRestore();
    });

    test('renders form fields', () => {
        expect(screen.getByPlaceholderText(/First Name/i)).toBeInTheDocument();
        expect(screen.getByPlaceholderText(/Last Name/i)).toBeInTheDocument();
        expect(screen.getByPlaceholderText(/Full Name/i)).toBeInTheDocument();
        expect(screen.getByPlaceholderText(/Date of Birth/i)).toBeInTheDocument();
        expect(screen.getByPlaceholderText(/Email/i)).toBeInTheDocument();
        expect(screen.getByPlaceholderText(/Contact Information/i)).toBeInTheDocument();
    });

    test('updates form fields on change', () => {
        fireEvent.change(screen.getByPlaceholderText(/First Name/i), { target: { value: 'John' } });
        fireEvent.change(screen.getByPlaceholderText(/Last Name/i), { target: { value: 'Doe' } });
        fireEvent.change(screen.getByPlaceholderText(/Full Name/i), { target: { value: 'John Doe' } });
        fireEvent.change(screen.getByPlaceholderText(/Date of Birth/i), { target: { value: '1990-01-01' } });
        fireEvent.change(screen.getByPlaceholderText(/Email/i), { target: { value: 'john.doe@example.com' } });
        fireEvent.change(screen.getByPlaceholderText(/Contact Information/i), { target: { value: '1234567890' } });

        expect(screen.getByPlaceholderText(/First Name/i).value).toBe('John');
        expect(screen.getByPlaceholderText(/Last Name/i).value).toBe('Doe');
        expect(screen.getByPlaceholderText(/Full Name/i).value).toBe('John Doe');
        expect(screen.getByPlaceholderText(/Date of Birth/i).value).toBe('1990-01-01');
        expect(screen.getByPlaceholderText(/Email/i).value).toBe('john.doe@example.com');
        expect(screen.getByPlaceholderText(/Contact Information/i).value).toBe('1234567890');
    });

    test('submits the form successfully', async () => {
        fetchApi.mockResolvedValueOnce({});

        fireEvent.change(screen.getByPlaceholderText(/First Name/i), { target: { value: 'John' } });
        fireEvent.change(screen.getByPlaceholderText(/Last Name/i), { target: { value: 'Doe' } });
        fireEvent.change(screen.getByPlaceholderText(/Full Name/i), { target: { value: 'John Doe' } });
        fireEvent.change(screen.getByPlaceholderText(/Date of Birth/i), { target: { value: '1990-01-01' } });
        fireEvent.change(screen.getByPlaceholderText(/Email/i), { target: { value: 'john.doe@example.com' } });
        fireEvent.change(screen.getByPlaceholderText(/Contact Information/i), { target: { value: '1234567890' } });

        fireEvent.submit(screen.getByRole('button', { name: /Update Profile/i }).closest('form'));

        await waitFor(() => {
            expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/updatePatient', {
                method: 'PUT',
                body: JSON.stringify({
                    firstName: 'John',
                    lastName: 'Doe',
                    fullName: 'John Doe',
                    dateOfBirth: '1990-01-01',
                    email: 'john.doe@example.com',
                    contactInformation: '1234567890'
                })
            });
        });

        expect(window.alert).toHaveBeenCalledWith('Profile updated successfully');
    });

    test('shows error message on form submission failure', async () => {
        fetchApi.mockRejectedValueOnce(new Error('Failed to update profile'));

        fireEvent.change(screen.getByPlaceholderText(/First Name/i), { target: { value: 'John' } });
        fireEvent.change(screen.getByPlaceholderText(/Last Name/i), { target: { value: 'Doe' } });
        fireEvent.change(screen.getByPlaceholderText(/Full Name/i), { target: { value: 'John Doe' } });
        fireEvent.change(screen.getByPlaceholderText(/Date of Birth/i), { target: { value: '1990-01-01' } });
        fireEvent.change(screen.getByPlaceholderText(/Email/i), { target: { value: 'john.doe@example.com' } });
        fireEvent.change(screen.getByPlaceholderText(/Contact Information/i), { target: { value: '1234567890' } });

        fireEvent.submit(screen.getByRole('button', { name: /Update Profile/i }).closest('form'));

        await waitFor(() => {
            expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/updatePatient', {
                method: 'PUT',
                body: JSON.stringify({
                    firstName: 'John',
                    lastName: 'Doe',
                    fullName: 'John Doe',
                    dateOfBirth: '1990-01-01',
                    email: 'john.doe@example.com',
                    contactInformation: '1234567890'
                })
            });
        });

        expect(window.alert).toHaveBeenCalledWith('Failed to update profile');
    });

    test('does not submit the form if user cancels confirmation', async () => {
        window.confirm.mockReturnValueOnce(false);

        fireEvent.change(screen.getByPlaceholderText(/First Name/i), { target: { value: 'John' } });
        fireEvent.change(screen.getByPlaceholderText(/Last Name/i), { target: { value: 'Doe' } });
        fireEvent.change(screen.getByPlaceholderText(/Full Name/i), { target: { value: 'John Doe' } });
        fireEvent.change(screen.getByPlaceholderText(/Date of Birth/i), { target: { value: '1990-01-01' } });
        fireEvent.change(screen.getByPlaceholderText(/Email/i), { target: { value: 'john.doe@example.com' } });
        fireEvent.change(screen.getByPlaceholderText(/Contact Information/i), { target: { value: '1234567890' } });

        fireEvent.submit(screen.getByRole('button', { name: /Update Profile/i }).closest('form'));

        await waitFor(() => {
            expect(fetchApi).not.toHaveBeenCalled();
        });

        expect(window.alert).not.toHaveBeenCalled();
    });
});