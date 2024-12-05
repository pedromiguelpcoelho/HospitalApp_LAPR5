import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import DeletePatientProfileForm from '../../../../Components/PatientProfile/DeletePatientProfileForm';
import Cookies from 'js-cookie';


jest.mock('js-cookie');

beforeEach(() => {
    jest.clearAllMocks();
});

test('renders DeletePatientProfileForm', () => {
    render(<DeletePatientProfileForm onDelete={jest.fn()} />);
    expect(screen.getByText('Select Parameter')).toBeInTheDocument();
    expect(screen.getByText('Delete Profile')).toBeInTheDocument();
});

test('updates input fields correctly', () => {
    render(<DeletePatientProfileForm onDelete={jest.fn()} />);
    fireEvent.change(screen.getByRole('combobox'), { target: { value: 'email' } });
    fireEvent.change(screen.getByPlaceholderText('Email'), { target: { value: 'john.doe@example.com' } });
    expect(screen.getByPlaceholderText('Email')).toHaveValue('john.doe@example.com');
});

test('submits form with valid data', () => {
    const onDelete = jest.fn();
    render(<DeletePatientProfileForm onDelete={onDelete} />);
    fireEvent.change(screen.getByRole('combobox'), { target: { value: 'email' } });
    fireEvent.change(screen.getByPlaceholderText('Email'), { target: { value: 'john.doe@example.com' } });
    fireEvent.click(screen.getByText('Delete Profile'));
    expect(onDelete).toHaveBeenCalledWith({ email: 'john.doe@example.com' });
});

test('fetches profile details on input change', async () => {
    Cookies.get.mockReturnValue('fake-token');
    const mockProfile = { fullName: 'John Doe', email: 'john.doe@example.com', medicalRecordNumber: '12345' };
    global.fetch = jest.fn(() =>
        Promise.resolve({
            ok: true,
            json: () => Promise.resolve([mockProfile]),
        })
    );

    render(<DeletePatientProfileForm onDelete={jest.fn()} />);
    fireEvent.change(screen.getByRole('combobox'), { target: { value: 'email' } });
    fireEvent.change(screen.getByPlaceholderText('Email'), { target: { value: 'john.doe@example.com' } });

    await waitFor(() => expect(screen.getByText('Full Name:')).toBeInTheDocument());
    expect(screen.getByText('John Doe')).toBeInTheDocument();
    expect(screen.getByText('Email:')).toBeInTheDocument();
    expect(screen.getByText('john.doe@example.com')).toBeInTheDocument();
    expect(screen.getByText('Medical Record Number:')).toBeInTheDocument();
    expect(screen.getByText('12345')).toBeInTheDocument();
});

test('handles fetch profile details error', async () => {
    Cookies.get.mockReturnValue('fake-token');
    global.fetch = jest.fn(() => Promise.reject('API is down'));
    const consoleErrorSpy = jest.spyOn(console, 'error').mockImplementation(() => {});

    render(<DeletePatientProfileForm onDelete={jest.fn()} />);
    fireEvent.change(screen.getByRole('combobox'), { target: { value: 'email' } });
    fireEvent.change(screen.getByPlaceholderText('Email'), { target: { value: 'john.doe@example.com' } });

    await waitFor(() => expect(screen.queryByText('Full Name:')).not.toBeInTheDocument());
    expect(screen.queryByText('Email:')).not.toBeInTheDocument();
    expect(screen.queryByText('Medical Record Number:')).not.toBeInTheDocument();

    consoleErrorSpy.mockRestore();
});
