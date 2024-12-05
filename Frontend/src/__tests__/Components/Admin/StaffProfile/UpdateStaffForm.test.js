import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import UpdateStaffForm from '../../../../Components/StaffProfile/UpdateStaffForm';

describe('UpdateStaffForm Component', () => {
    let mockOnFormChange;
    let mockOnUpdate;
    let formData;

    beforeEach(() => {
        mockOnFormChange = jest.fn();
        mockOnUpdate = jest.fn();
        formData = {
            firstName: 'John',
            lastName: 'Doe',
            email: 'john.doe@example.com',
            specialization: 'Orthopaedist',
            role: 'Doctor',
            phoneNumber: '1234567890'
        };
    });

    test('renders form with populated fields when formData is provided', () => {
        render(
            <UpdateStaffForm
                onFormChange={mockOnFormChange}
                onUpdate={mockOnUpdate}
                formData={formData}
                loading={false}
            />
        );

       
        expect(screen.getByPlaceholderText('First Name')).toHaveValue('John');
        expect(screen.getByPlaceholderText('Last Name')).toHaveValue('Doe');
        expect(screen.getByPlaceholderText('Email')).toHaveValue('john.doe@example.com');
        expect(screen.getByPlaceholderText('Specialization')).toHaveValue('Orthopaedist');
        expect(screen.getByPlaceholderText('Role')).toHaveValue('Doctor');
        expect(screen.getByPlaceholderText('Phone Number')).toHaveValue('1234567890');
    });

    test('does not render form when formData is null', () => {
        render(
            <UpdateStaffForm
                onFormChange={mockOnFormChange}
                onUpdate={mockOnUpdate}
                formData={null}
                loading={false}
            />
        );

       
        expect(screen.queryByPlaceholderText('First Name')).not.toBeInTheDocument();
    });

    test('calls onFormChange when input fields are modified', () => {
        render(
            <UpdateStaffForm
                onFormChange={mockOnFormChange}
                onUpdate={mockOnUpdate}
                formData={formData}
                loading={false}
            />
        );

        
        const firstNameInput = screen.getByPlaceholderText('First Name');
        fireEvent.change(firstNameInput, { target: { value: 'Jane' } });

        
        expect(mockOnFormChange).toHaveBeenCalledTimes(1);
        expect(mockOnFormChange).toHaveBeenCalledWith(expect.anything()); // Passes the event object
    });

    test('calls onUpdate when Update button is clicked', () => {
        render(
            <UpdateStaffForm
                onFormChange={mockOnFormChange}
                onUpdate={mockOnUpdate}
                formData={formData}
                loading={false}
            />
        );

        
        const updateButton = screen.getByText('Update');
        fireEvent.click(updateButton);

       
        expect(mockOnUpdate).toHaveBeenCalledTimes(1);
    });

    test('disables Update button when loading is true', () => {
        render(
            <UpdateStaffForm
                onFormChange={mockOnFormChange}
                onUpdate={mockOnUpdate}
                formData={formData}
                loading={true}
            />
        );

       
        const updateButton = screen.getByText('Update');
        expect(updateButton).toBeDisabled();
    });
});
