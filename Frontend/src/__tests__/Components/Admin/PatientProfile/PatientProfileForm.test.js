import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import PatientProfileForm from '../../../../Components/PatientProfile/PatientProfileForm';

test('renders PatientProfileForm', () => {
    render(<PatientProfileForm onCreateProfile={jest.fn()} />);
    expect(screen.getByText('Add Patient Profile')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('First Name')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Last Name')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Full Name')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Date of Birth')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Email')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Contact Information')).toBeInTheDocument();
});

test('updates input fields correctly', () => {
    render(<PatientProfileForm onCreateProfile={jest.fn()} />);
    fireEvent.change(screen.getByPlaceholderText('First Name'), { target: { value: 'John' } });
    fireEvent.change(screen.getByPlaceholderText('Last Name'), { target: { value: 'Doe' } });
    fireEvent.change(screen.getByPlaceholderText('Full Name'), { target: { value: 'John Doe' } });
    fireEvent.change(screen.getByPlaceholderText('Date of Birth'), { target: { value: '2000-01-01' } });
    fireEvent.change(screen.getByPlaceholderText('Email'), { target: { value: 'john.doe@example.com' } });
    fireEvent.change(screen.getByPlaceholderText('Contact Information'), { target: { value: '123456789' } });

    expect(screen.getByPlaceholderText('First Name')).toHaveValue('John');
    expect(screen.getByPlaceholderText('Last Name')).toHaveValue('Doe');
    expect(screen.getByPlaceholderText('Full Name')).toHaveValue('John Doe');
    expect(screen.getByPlaceholderText('Date of Birth')).toHaveValue('2000-01-01');
    expect(screen.getByPlaceholderText('Email')).toHaveValue('john.doe@example.com');
    expect(screen.getByPlaceholderText('Contact Information')).toHaveValue('123456789');
});

test('submits form with valid data', () => {
    const onCreateProfile = jest.fn();
    render(<PatientProfileForm onCreateProfile={onCreateProfile} />);

    fireEvent.change(screen.getByPlaceholderText('First Name'), { target: { value: 'John' } });
    fireEvent.change(screen.getByPlaceholderText('Last Name'), { target: { value: 'Doe' } });
    fireEvent.change(screen.getByPlaceholderText('Full Name'), { target: { value: 'John Doe' } });
    fireEvent.change(screen.getByPlaceholderText('Date of Birth'), { target: { value: '2000-01-01' } });
    fireEvent.change(screen.getByPlaceholderText('Email'), { target: { value: 'john.doe@example.com' } });
    fireEvent.change(screen.getByPlaceholderText('Contact Information'), { target: { value: '123456789' } });

    fireEvent.click(screen.getByText('Create'));

    expect(onCreateProfile).toHaveBeenCalledWith({
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        dateOfBirth: '2000-01-01',
        email: 'john.doe@example.com',
        contactInformation: '123456789',
    });
});
test('renders PatientProfileForm', () => {
    render(<PatientProfileForm onCreateProfile={jest.fn()} />);
    expect(screen.getByText('Add Patient Profile')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('First Name')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Last Name')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Full Name')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Date of Birth')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Email')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Contact Information')).toBeInTheDocument();
});

test('updates input fields correctly', () => {
    render(<PatientProfileForm onCreateProfile={jest.fn()} />);
    fireEvent.change(screen.getByPlaceholderText('First Name'), { target: { value: 'John' } });
    fireEvent.change(screen.getByPlaceholderText('Last Name'), { target: { value: 'Doe' } });
    fireEvent.change(screen.getByPlaceholderText('Full Name'), { target: { value: 'John Doe' } });
    fireEvent.change(screen.getByPlaceholderText('Date of Birth'), { target: { value: '2000-01-01' } });
    fireEvent.change(screen.getByPlaceholderText('Email'), { target: { value: 'john.doe@example.com' } });
    fireEvent.change(screen.getByPlaceholderText('Contact Information'), { target: { value: '123456789' } });

    expect(screen.getByPlaceholderText('First Name')).toHaveValue('John');
    expect(screen.getByPlaceholderText('Last Name')).toHaveValue('Doe');
    expect(screen.getByPlaceholderText('Full Name')).toHaveValue('John Doe');
    expect(screen.getByPlaceholderText('Date of Birth')).toHaveValue('2000-01-01');
    expect(screen.getByPlaceholderText('Email')).toHaveValue('john.doe@example.com');
    expect(screen.getByPlaceholderText('Contact Information')).toHaveValue('123456789');
});

test('submits form with valid data', () => {
    const onCreateProfile = jest.fn();
    render(<PatientProfileForm onCreateProfile={onCreateProfile} />);

    fireEvent.change(screen.getByPlaceholderText('First Name'), { target: { value: 'John' } });
    fireEvent.change(screen.getByPlaceholderText('Last Name'), { target: { value: 'Doe' } });
    fireEvent.change(screen.getByPlaceholderText('Full Name'), { target: { value: 'John Doe' } });
    fireEvent.change(screen.getByPlaceholderText('Date of Birth'), { target: { value: '2000-01-01' } });
    fireEvent.change(screen.getByPlaceholderText('Email'), { target: { value: 'john.doe@example.com' } });
    fireEvent.change(screen.getByPlaceholderText('Contact Information'), { target: { value: '123456789' } });

    fireEvent.click(screen.getByText('Create'));

    expect(onCreateProfile).toHaveBeenCalledWith({
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        dateOfBirth: '2000-01-01',
        email: 'john.doe@example.com',
        contactInformation: '123456789',
    });
});


