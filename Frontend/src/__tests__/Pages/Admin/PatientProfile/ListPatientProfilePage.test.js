import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import ListPatientProfilePage from '../../../../Pages/Admin/PatientProfile/ListPatientProfilePage';
import fetchApi from '../../../../Services/fetchApi';

jest.mock('../../../../Services/fetchApi');

describe('ListPatientProfilePage', () => {
    beforeEach(() => {
        fetchApi.mockClear();
    });

    test('renders without crashing', () => {
        render(<ListPatientProfilePage />);
        expect(screen.getByText('Patient Profiles')).toBeInTheDocument();
    });

    test('displays error message when no profiles are found', async () => {
        fetchApi.mockResolvedValueOnce([]);
        render(<ListPatientProfilePage />);
    
        fireEvent.change(screen.getByRole('combobox'), { target: { value: 'fullName' } });
        fireEvent.change(screen.getByPlaceholderText('Full Name'), { target: { value: 'John Doe' } });
        fireEvent.click(screen.getByText('Search'));
    
        await waitFor(() => expect(screen.getByText('No patient profiles found.')).toBeInTheDocument());
    });
    
    test('displays profiles when search is successful', async () => {
        const mockProfiles = [
            { id: 1, fullName: 'John Doe', medicalRecordNumber: '123', dateOfBirth: '1990-01-01', email: 'john@example.com', phoneNumber: '1234567890' },
            { id: 2, fullName: 'Jane Doe', medicalRecordNumber: '456', dateOfBirth: '1992-02-02', email: 'jane@example.com', phoneNumber: '0987654321' }
        ];
        fetchApi.mockResolvedValueOnce(mockProfiles);
        render(<ListPatientProfilePage />);
    
        fireEvent.change(screen.getByRole('combobox'), { target: { value: 'fullName' } });
        fireEvent.change(screen.getByPlaceholderText('Full Name'), { target: { value: 'Doe' } });
        fireEvent.click(screen.getByText('Search'));
    
        await waitFor(() => {
            expect(screen.getByText('John Doe')).toBeInTheDocument();
            expect(screen.getByText('Jane Doe')).toBeInTheDocument();
        });
    });
    
    test('displays loading state during fetch', async () => {
        fetchApi.mockImplementation(() => new Promise(resolve => setTimeout(() => resolve([]), 100)));
        render(<ListPatientProfilePage />);
    
        fireEvent.change(screen.getByRole('combobox'), { target: { value: 'fullName' } });
        fireEvent.change(screen.getByPlaceholderText('Full Name'), { target: { value: 'John Doe' } });
        fireEvent.click(screen.getByText('Search'));
    
        expect(screen.getByText('Loading...')).toBeInTheDocument();
        await waitFor(() => expect(screen.queryByText('Loading...')).not.toBeInTheDocument());
    });
    
    test('displays selected profile details when a profile is clicked', async () => {
        const mockProfiles = [
            { id: 1, fullName: 'John Doe', firstName: 'John', lastName: 'Doe', medicalRecordNumber: '123', dateOfBirth: '1990-01-01', email: 'john@example.com', contactInformation: '1234567890' }
        ];
        fetchApi.mockResolvedValueOnce(mockProfiles);
        render(<ListPatientProfilePage />);
    
        fireEvent.change(screen.getByRole('combobox'), { target: { value: 'fullName' } });
        fireEvent.change(screen.getByPlaceholderText('Full Name'), { target: { value: 'Doe' } });
        fireEvent.click(screen.getByText('Search'));
    
        await waitFor(() => {
            fireEvent.click(screen.getAllByText('John Doe')[0]);
            expect(screen.getByText(/Patient Details/i)).toBeInTheDocument();
    
            // Use textContent matcher for nested elements
            expect(screen.getByText((_, node) => node.textContent === 'First Name: John')).toBeInTheDocument();
            expect(screen.getByText((_, node) => node.textContent === 'Last Name: Doe')).toBeInTheDocument();
            expect(screen.getByText((_, node) => node.textContent === 'Full Name: John Doe')).toBeInTheDocument();
            expect(screen.getByText((_, node) => node.textContent === 'Email: john@example.com')).toBeInTheDocument();
            expect(screen.getByText((_, node) => node.textContent === 'Date of Birth: 1/1/1990')).toBeInTheDocument();
            expect(screen.getByText((_, node) => node.textContent === 'Contact Information: 1234567890')).toBeInTheDocument();
            expect(screen.getByText((_, node) => node.textContent === 'Medical Record Number: 123')).toBeInTheDocument();
        });
    });    
});