import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import StaffList from '../../../../Components/StaffProfile/StaffList';

describe('StaffList Component', () => {
    let mockOnSearchChange;
    let mockOnFilterChange;
    let mockOnSearchSubmit;
    let searchParams;

    beforeEach(() => {
        mockOnSearchChange = jest.fn();
        mockOnFilterChange = jest.fn();
        mockOnSearchSubmit = jest.fn();
        searchParams = {};
    });

    test('renders filter dropdown and search button', () => {
        render(
            <StaffList
                searchParams={searchParams}
                selectedFilter=""
                onSearchChange={mockOnSearchChange}
                onFilterChange={mockOnFilterChange}
                onSearchSubmit={mockOnSearchSubmit}
            />
        );

        
        expect(screen.getByText('Select Filter')).toBeInTheDocument();
        expect(screen.getByText('Search')).toBeInTheDocument();
    });

    test('renders appropriate input field based on selected filter', () => {
        render(
            <StaffList
                searchParams={searchParams}
                selectedFilter="name"
                onSearchChange={mockOnSearchChange}
                onFilterChange={mockOnFilterChange}
                onSearchSubmit={mockOnSearchSubmit}
            />
        );

       
        const nameInput = screen.getByPlaceholderText('Name');
        expect(nameInput).toBeInTheDocument();
        expect(nameInput).toHaveAttribute('type', 'text');

       
        render(
            <StaffList
                searchParams={searchParams}
                selectedFilter="isActive"
                onSearchChange={mockOnSearchChange}
                onFilterChange={mockOnFilterChange}
                onSearchSubmit={mockOnSearchSubmit}
            />
        );

       
        const isActiveCheckbox = screen.getByRole('checkbox');
        expect(isActiveCheckbox).toBeInTheDocument();
    });

    test('calls onFilterChange when a filter is selected', () => {
        render(
            <StaffList
                searchParams={searchParams}
                selectedFilter=""
                onSearchChange={mockOnSearchChange}
                onFilterChange={mockOnFilterChange}
                onSearchSubmit={mockOnSearchSubmit}
            />
        );

       
         
        const filterDropdown = screen.getByRole('combobox'); 
        fireEvent.change(filterDropdown, { target: { value: 'email' } });

        
        expect(mockOnFilterChange).toHaveBeenCalledTimes(1);
        expect(mockOnFilterChange).toHaveBeenCalledWith(expect.anything()); 
    });

    test('calls onSearchChange when search input value changes', () => {
        render(
            <StaffList
                searchParams={searchParams}
                selectedFilter="name"
                onSearchChange={mockOnSearchChange}
                onFilterChange={mockOnFilterChange}
                onSearchSubmit={mockOnSearchSubmit}
            />
        );

       
        const nameInput = screen.getByPlaceholderText('Name');
        fireEvent.change(nameInput, { target: { value: 'John' } });

        
        expect(mockOnSearchChange).toHaveBeenCalledTimes(1);
        expect(mockOnSearchChange).toHaveBeenCalledWith(expect.anything());
    });

    test('calls onSearchSubmit when search button is clicked', () => {
        render(
            <StaffList
                searchParams={searchParams}
                selectedFilter="name"
                onSearchChange={mockOnSearchChange}
                onFilterChange={mockOnFilterChange}
                onSearchSubmit={mockOnSearchSubmit}
            />
        );

        
        const searchButton = screen.getByText('Search');
        fireEvent.click(searchButton);

        
        expect(mockOnSearchSubmit).toHaveBeenCalledTimes(1);
    });
});
