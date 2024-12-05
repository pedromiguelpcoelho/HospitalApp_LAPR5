import React from 'react';
import { render, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import UpdateMyOwnPatientProfileForm from '../../../Components/PatientProfile/UpdateMyOwnPatientProfileForm';

describe('UpdateMyOwnPatientProfileForm', () => {
    const formData = {
        firstName: '',
        lastName: '',
        fullName: '',
        dateOfBirth: '',
        email: '',
        contactInformation: ''
    };

    const handleChange = jest.fn();
    const handleSubmit = jest.fn((e) => e.preventDefault());

    test('renders form inputs correctly', () => {
        const { getByPlaceholderText } = render(
            <UpdateMyOwnPatientProfileForm formData={formData} handleChange={handleChange} handleSubmit={handleSubmit} />
        );

        expect(getByPlaceholderText('First Name')).toBeInTheDocument();
        expect(getByPlaceholderText('Last Name')).toBeInTheDocument();
        expect(getByPlaceholderText('Full Name')).toBeInTheDocument();
        expect(getByPlaceholderText('Date of Birth')).toBeInTheDocument();
        expect(getByPlaceholderText('Email')).toBeInTheDocument();
        expect(getByPlaceholderText('Contact Information')).toBeInTheDocument();
    });

    test('calls handleChange on input change', () => {
        const { getByPlaceholderText } = render(
            <UpdateMyOwnPatientProfileForm formData={formData} handleChange={handleChange} handleSubmit={handleSubmit} />
        );

        fireEvent.change(getByPlaceholderText('First Name'), { target: { value: 'John' } });
        expect(handleChange).toHaveBeenCalled();
    });

    test('calls handleSubmit on form submit', () => {
        const { getByText } = render(
            <UpdateMyOwnPatientProfileForm formData={formData} handleChange={handleChange} handleSubmit={handleSubmit} />
        );

        fireEvent.submit(getByText('Update Profile').closest('form'));
        expect(handleSubmit).toHaveBeenCalled();
    });
});