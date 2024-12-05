import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import PatientProfilesList from '../../../../Components/PatientProfile/PatientProfileList';

test('renders PatientProfilesList', () => {
    render(<PatientProfilesList searchParams={{}} selectedFilter="" onSearchChange={jest.fn()} onFilterChange={jest.fn()} />);
    expect(screen.getByText('Select Filter')).toBeInTheDocument();
});

test('updates filter selection correctly', () => {
    const onFilterChange = jest.fn();
    render(<PatientProfilesList searchParams={{}} selectedFilter="" onSearchChange={jest.fn()} onFilterChange={onFilterChange} />);
    fireEvent.change(screen.getByRole('combobox'), { target: { value: 'fullName' } });
    expect(onFilterChange).toHaveBeenCalledWith(expect.any(Object));
});

test('updates input field correctly', () => {
    const onSearchChange = jest.fn();
    render(<PatientProfilesList searchParams={{}} selectedFilter="fullName" onSearchChange={onSearchChange} onFilterChange={jest.fn()} />);
    const input = screen.getByPlaceholderText('Full Name');
    fireEvent.change(input, { target: { value: 'John Doe' } });
    expect(onSearchChange).toHaveBeenCalledWith(expect.any(Object));
});

test('displays correct input field based on selected filter', () => {
    render(<PatientProfilesList searchParams={{}} selectedFilter="dateOfBirth" onSearchChange={jest.fn()} onFilterChange={jest.fn()} />);
    const input = screen.getByPlaceholderText('Date Of Birth');
    expect(input).toHaveAttribute('type', 'date');
});
test('renders PatientProfilesList', () => {
    render(<PatientProfilesList searchParams={{}} selectedFilter="" onSearchChange={jest.fn()} onFilterChange={jest.fn()} />);
    expect(screen.getByText('Select Filter')).toBeInTheDocument();
});

test('updates filter selection correctly', () => {
    const onFilterChange = jest.fn();
    render(<PatientProfilesList searchParams={{}} selectedFilter="" onSearchChange={jest.fn()} onFilterChange={onFilterChange} />);
    fireEvent.change(screen.getByRole('combobox'), { target: { value: 'fullName' } });
    expect(onFilterChange).toHaveBeenCalledWith(expect.any(Object));
});

test('updates input field correctly', () => {
    const onSearchChange = jest.fn();
    render(<PatientProfilesList searchParams={{}} selectedFilter="fullName" onSearchChange={onSearchChange} onFilterChange={jest.fn()} />);
    const input = screen.getByPlaceholderText('Full Name');
    fireEvent.change(input, { target: { value: 'John Doe' } });
    expect(onSearchChange).toHaveBeenCalledWith(expect.any(Object));
});

test('displays correct input field based on selected filter', () => {
    render(<PatientProfilesList searchParams={{}} selectedFilter="dateOfBirth" onSearchChange={jest.fn()} onFilterChange={jest.fn()} />);
    const input = screen.getByPlaceholderText('Date Of Birth');
    expect(input).toHaveAttribute('type', 'date');
});

test('input field is not displayed when no filter is selected', () => {
    render(<PatientProfilesList searchParams={{}} selectedFilter="" onSearchChange={jest.fn()} onFilterChange={jest.fn()} />);
    expect(screen.queryByRole('textbox')).toBeNull();
});

test('input field displays correct value from searchParams', () => {
    render(<PatientProfilesList searchParams={{ fullName: 'Jane Doe' }} selectedFilter="fullName" onSearchChange={jest.fn()} onFilterChange={jest.fn()} />);
    const input = screen.getByPlaceholderText('Full Name');
    expect(input).toHaveValue('Jane Doe');
});
