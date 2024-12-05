// RequestDeletionOfPatientProfileForm.test.js
import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import RequestDeletionOfPatientProfileForm from '../../../Components/PatientProfile/RequestDeletionOfPatientProfileForm';

test('renders RequestDeletionOfPatientProfileForm', () => {
    render(<RequestDeletionOfPatientProfileForm handleSubmit={jest.fn()} />);
    expect(screen.getByText('Request Account Deletion')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /request deletion/i })).toBeInTheDocument();
});

test('submits form with valid data', () => {
    const handleSubmit = jest.fn();
    render(<RequestDeletionOfPatientProfileForm handleSubmit={handleSubmit} />);
    fireEvent.submit(screen.getByRole('button', { name: /request deletion/i }).closest('form'));
    expect(handleSubmit).toHaveBeenCalled();
});
// RequestDeletionOfPatientProfileForm.test.js

test('renders RequestDeletionOfPatientProfileForm', () => {
    render(<RequestDeletionOfPatientProfileForm handleSubmit={jest.fn()} />);
    expect(screen.getByText('Request Account Deletion')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /request deletion/i })).toBeInTheDocument();
});

test('submits form with valid data', () => {
    const handleSubmit = jest.fn();
    render(<RequestDeletionOfPatientProfileForm handleSubmit={handleSubmit} />);
    fireEvent.submit(screen.getByRole('button', { name: /request deletion/i }).closest('form'));
    expect(handleSubmit).toHaveBeenCalled();
});

test('button has correct type attribute', () => {
    render(<RequestDeletionOfPatientProfileForm handleSubmit={jest.fn()} />);
    expect(screen.getByRole('button', { name: /request deletion/i })).toHaveAttribute('type', 'submit');
});

test('form calls handleSubmit on submit', () => {
    const handleSubmit = jest.fn();
    render(<RequestDeletionOfPatientProfileForm handleSubmit={handleSubmit} />);
    fireEvent.submit(screen.getByRole('button', { name: /request deletion/i }).closest('form'));
    expect(handleSubmit).toHaveBeenCalledTimes(1);
});
