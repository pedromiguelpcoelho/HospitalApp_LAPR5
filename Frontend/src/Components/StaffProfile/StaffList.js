import React from 'react';

function StaffList({ searchParams, selectedFilter, onSearchChange, onFilterChange, onSearchSubmit }) {
    return (
        <div className="staff-list">
            <select value={selectedFilter} onChange={onFilterChange}>
                <option value="">Select Filter</option>
                <option value="name">Name</option>
                <option value="specialization">Specialization</option>
                <option value="email">Email</option>
                <option value="phoneNumber">Phone Number</option>
                <option value="licenseNumber">License Number</option>
                <option value="isActive">Active Status</option>
            </select>

            {selectedFilter && (
                <input
                    type={selectedFilter === 'isActive' ? 'checkbox' : 'text'}
                    name={selectedFilter}
                    placeholder={selectedFilter.charAt(0).toUpperCase() + selectedFilter.slice(1).replace(/([A-Z])/g, ' $1')}
                    value={selectedFilter === 'isActive' ? undefined : searchParams[selectedFilter] || ''}
                    checked={selectedFilter === 'isActive' ? searchParams[selectedFilter] || false : undefined}
                    onChange={onSearchChange}
                />
            )}

            <button onClick={onSearchSubmit}>Search</button>
        </div>
    );
}

export default StaffList;
