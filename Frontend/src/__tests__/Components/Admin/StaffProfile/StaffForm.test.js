import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import StaffForm from '../../../../Components/StaffProfile/StaffForm';

describe('StaffForm Component', () => {
    let mockHandleInputChange;
    let mockOnCreateStaffMember;
    let staffData;

    beforeEach(() => {
        mockHandleInputChange = jest.fn();
        mockOnCreateStaffMember = jest.fn();
        staffData = {
            firstName: '',
            lastName: '',
            email: '',
            phoneNumber: '',
            role: '',
            specialization: ''
        };
    });

    test('renders form with all fields', () => {
        render(
            <StaffForm
                onCreateStaffMember={mockOnCreateStaffMember}
                staffData={staffData}
                handleInputChange={mockHandleInputChange}
            />
        );

        
        expect(screen.getByPlaceholderText('First Name')).toBeInTheDocument();
        expect(screen.getByPlaceholderText('Last Name')).toBeInTheDocument();
        expect(screen.getByPlaceholderText('Email')).toBeInTheDocument();
        expect(screen.getByPlaceholderText('Phone Number')).toBeInTheDocument();

        
        expect(screen.getByLabelText('Role')).toBeInTheDocument();

        
        expect(screen.getByText('Create')).toBeInTheDocument();
    });

    test('updates specialization options when role changes', () => {
        render(
            <StaffForm
                onCreateStaffMember={mockOnCreateStaffMember}
                staffData={{ ...staffData, role: 'Doctor' }}
                handleInputChange={mockHandleInputChange}
            />
        );

        
        fireEvent.change(screen.getByLabelText('Role'), { target: { value: 'Doctor' } });
        expect(mockHandleInputChange).toHaveBeenCalled();

        render(
            <StaffForm
                onCreateStaffMember={mockOnCreateStaffMember}
                staffData={{ ...staffData, role: 'Nurse' }}
                handleInputChange={mockHandleInputChange}
            />
        );

        
        const specializationDropdown = screen.getByLabelText('Specialization');
        expect(specializationDropdown).toBeInTheDocument();
        expect(screen.getByText('Instrumenting Nurse')).toBeInTheDocument();
    });

    test('calls onCreateStaffMember with correct data on form submission', () => {
        staffData = {
            firstName: 'John',
            lastName: 'Doe',
            email: 'john.doe@example.com',
            phoneNumber: '1234567890',
            role: 'Doctor',
            specialization: 'Orthopaedist'
        };

        render(
            <StaffForm
                onCreateStaffMember={mockOnCreateStaffMember}
                staffData={staffData}
                handleInputChange={mockHandleInputChange}
            />
        );

        
        fireEvent.click(screen.getByText('Create'));

        
        expect(mockOnCreateStaffMember).toHaveBeenCalledWith(staffData);
    });

    test('does not submit form with missing required fields', () => {
        render(
            <StaffForm
                onCreateStaffMember={mockOnCreateStaffMember}
                staffData={{ ...staffData, role: 'Doctor' }}
                handleInputChange={mockHandleInputChange}
            />
        );

        
        fireEvent.click(screen.getByText('Create'));

        
        expect(mockOnCreateStaffMember).not.toHaveBeenCalled();
    });
});
