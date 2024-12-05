import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import DeleteStaffForm from '../../../../Components/StaffProfile/DeleteStaffForm';

describe('DeleteStaffForm', () => {
    const mockOnDelete = jest.fn();
    const staffList = [
        { id: '1', name: 'John Doe', email: 'john.doe@example.com', phoneNumber: '987654321', specialization: 'Cardiology', licenseNumber: '12345', isActive: true },
        { id: '2', name: 'Jane Smith', email: 'jane.smith@example.com', phoneNumber: '978654321', specialization: 'Neurology', licenseNumber: '67890', isActive: false },
    ];

    beforeEach(() => {
        mockOnDelete.mockClear();
    });

    test('renders the form with all fields', () => {
        render(<DeleteStaffForm staffList={staffList} onDelete={mockOnDelete} />);

        
        expect(screen.getByText('Select Staff ID:')).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /deactivate/i })).toBeInTheDocument();

        
        expect(screen.getByText('1 - John Doe')).toBeInTheDocument();
        expect(screen.getByText('2 - Jane Smith')).toBeInTheDocument();
    });

    test('displays the selected staff details', () => {
        render(<DeleteStaffForm staffList={staffList} onDelete={mockOnDelete} />);

        
        fireEvent.change(screen.getByRole('combobox'), { target: { value: '1' } });

        const detailsContainer = screen.getByText('Staff Details').parentElement;

        
        expect(detailsContainer).toHaveTextContent('ID: 1');
        expect(detailsContainer).toHaveTextContent('Full Name: John Doe');
        expect(detailsContainer).toHaveTextContent('Email: john.doe@example.com');
        expect(detailsContainer).toHaveTextContent('Phone Number: 987654321');
        expect(detailsContainer).toHaveTextContent('Specialization: Cardiology');
        expect(detailsContainer).toHaveTextContent('License Number: 12345');
        expect(detailsContainer).toHaveTextContent('Status: Active');
    });

    test('calls onDelete with the correct ID when the form is submitted', () => {
        render(<DeleteStaffForm staffList={staffList} onDelete={mockOnDelete} />);

        
        fireEvent.change(screen.getByRole('combobox'), { target: { value: '2' } });

        
        fireEvent.click(screen.getByRole('button', { name: /deactivate/i }));

        
        expect(mockOnDelete).toHaveBeenCalledWith('2');
    });

   
});



