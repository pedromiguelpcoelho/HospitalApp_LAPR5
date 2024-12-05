import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import UpdatePatientProfileForm from '../../../../Components/PatientProfile/UpdatePatientProfileForm';

test('renders UpdatePatientProfileForm', () => {
    render(<UpdatePatientProfileForm searchParams={{}} selectedFilter="" onSearchChange={jest.fn()} onFilterChange={jest.fn()} onFormChange={jest.fn()} onSearch={jest.fn()} onUpdate={jest.fn()} formData={null} loading={false} />);
    expect(screen.getByText('Select Filter')).toBeInTheDocument();
    expect(screen.getByText('Search')).toBeInTheDocument();
});

test('updates input fields correctly', () => {
    const searchParams = { email: '' };
    const onSearchChange = jest.fn();
    const onFilterChange = jest.fn();
    render(<UpdatePatientProfileForm searchParams={searchParams} selectedFilter="email" onSearchChange={onSearchChange} onFilterChange={onFilterChange} onFormChange={jest.fn()} onSearch={jest.fn()} onUpdate={jest.fn()} formData={null} loading={false} />);
    
    fireEvent.change(screen.getByPlaceholderText('Email'), { target: { value: 'john.doe@example.com' } });
    expect(onSearchChange).toHaveBeenCalledWith(expect.any(Object));
});

test('submits form with valid data', () => {
    const formData = {
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        email: 'john.doe@example.com',
        contactInformation: '123456789',
        dateOfBirth: '2000-01-01',
        medicalRecordNumber: 'MRN123'
    };
    const onUpdate = jest.fn();
    render(<UpdatePatientProfileForm searchParams={{}} selectedFilter="" onSearchChange={jest.fn()} onFilterChange={jest.fn()} onFormChange={jest.fn()} onSearch={jest.fn()} onUpdate={onUpdate} formData={formData} loading={false} />);
    
    fireEvent.click(screen.getByText('Update'));
    expect(onUpdate).toHaveBeenCalled();
});
test('renders UpdatePatientProfileForm', () => {
    render(<UpdatePatientProfileForm searchParams={{}} selectedFilter="" onSearchChange={jest.fn()} onFilterChange={jest.fn()} onFormChange={jest.fn()} onSearch={jest.fn()} onUpdate={jest.fn()} formData={null} loading={false} />);
    expect(screen.getByText('Select Filter')).toBeInTheDocument();
    expect(screen.getByText('Search')).toBeInTheDocument();
});

test('updates input fields correctly', () => {
    const searchParams = { email: '' };
    const onSearchChange = jest.fn();
    const onFilterChange = jest.fn();
    render(<UpdatePatientProfileForm searchParams={searchParams} selectedFilter="email" onSearchChange={onSearchChange} onFilterChange={onFilterChange} onFormChange={jest.fn()} onSearch={jest.fn()} onUpdate={jest.fn()} formData={null} loading={false} />);
    
    fireEvent.change(screen.getByPlaceholderText('Email'), { target: { value: 'john.doe@example.com' } });
    expect(onSearchChange).toHaveBeenCalledWith(expect.any(Object));
});

test('submits form with valid data', () => {
    const formData = {
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        email: 'john.doe@example.com',
        contactInformation: '123456789',
        dateOfBirth: '2000-01-01',
        medicalRecordNumber: 'MRN123'
    };
    const onUpdate = jest.fn();
    render(<UpdatePatientProfileForm searchParams={{}} selectedFilter="" onSearchChange={jest.fn()} onFilterChange={jest.fn()} onFormChange={jest.fn()} onSearch={jest.fn()} onUpdate={onUpdate} formData={formData} loading={false} />);
    
    fireEvent.click(screen.getByText('Update'));
    expect(onUpdate).toHaveBeenCalled();
});

test('disables search button when loading', () => {
    render(<UpdatePatientProfileForm searchParams={{}} selectedFilter="" onSearchChange={jest.fn()} onFilterChange={jest.fn()} onFormChange={jest.fn()} onSearch={jest.fn()} onUpdate={jest.fn()} formData={null} loading={true} />);
    expect(screen.getByText('Search')).toBeDisabled();
});

test('disables update button when loading', () => {
    const formData = {
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        email: 'john.doe@example.com',
        contactInformation: '123456789',
        dateOfBirth: '2000-01-01',
        medicalRecordNumber: 'MRN123'
    };
    render(<UpdatePatientProfileForm searchParams={{}} selectedFilter="" onSearchChange={jest.fn()} onFilterChange={jest.fn()} onFormChange={jest.fn()} onSearch={jest.fn()} onUpdate={jest.fn()} formData={formData} loading={true} />);
    expect(screen.getByText('Update')).toBeDisabled();
});
