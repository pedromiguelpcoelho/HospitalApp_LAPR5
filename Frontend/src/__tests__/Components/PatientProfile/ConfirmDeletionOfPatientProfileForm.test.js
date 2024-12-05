import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import ConfirmDeletionOfPatientProfileForm from '../../../Components/PatientProfile/ConfirmDeletionOfPatientProfileForm';

test('renders ConfirmDeletionOfPatientProfileForm', () => {
    render(<ConfirmDeletionOfPatientProfileForm handleSubmit={jest.fn()} />);
    expect(screen.getByText('Confirm Account Deletion')).toBeInTheDocument();
    expect(screen.getByLabelText('Confirmation Token:')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /confirm deletion/i })).toBeInTheDocument();
});

test('updates input field correctly', () => {
    render(<ConfirmDeletionOfPatientProfileForm handleSubmit={jest.fn()} />);
    const input = screen.getByLabelText('Confirmation Token:');
    fireEvent.change(input, { target: { value: '123456' } });
    expect(input).toHaveValue('123456');
});

test('submits form with valid data', () => {
    const handleSubmit = jest.fn();
    render(<ConfirmDeletionOfPatientProfileForm handleSubmit={handleSubmit} />);
    const input = screen.getByLabelText('Confirmation Token:');
    fireEvent.change(input, { target: { value: '123456' } });
    fireEvent.click(screen.getByRole('button', { name: /confirm deletion/i }));
    expect(handleSubmit).toHaveBeenCalledWith('123456');
});
test('renders ConfirmDeletionOfPatientProfileForm', () => {
    render(<ConfirmDeletionOfPatientProfileForm handleSubmit={jest.fn()} />);
    expect(screen.getByText('Confirm Account Deletion')).toBeInTheDocument();
    expect(screen.getByLabelText('Confirmation Token:')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /confirm deletion/i })).toBeInTheDocument();
});

test('updates input field correctly', () => {
    render(<ConfirmDeletionOfPatientProfileForm handleSubmit={jest.fn()} />);
    const input = screen.getByLabelText('Confirmation Token:');
    fireEvent.change(input, { target: { value: '123456' } });
    expect(input).toHaveValue('123456');
});

test('submits form with valid data', () => {
    const handleSubmit = jest.fn();
    render(<ConfirmDeletionOfPatientProfileForm handleSubmit={handleSubmit} />);
    const input = screen.getByLabelText('Confirmation Token:');
    fireEvent.change(input, { target: { value: '123456' } });
    fireEvent.click(screen.getByRole('button', { name: /confirm deletion/i }));
    expect(handleSubmit).toHaveBeenCalledWith('123456');
});

