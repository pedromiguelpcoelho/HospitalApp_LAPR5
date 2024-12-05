import React from 'react';

function PatientProfilesList({ searchParams, selectedFilter, onSearchChange, onFilterChange }) {
    return (
        <div>
            <select value={selectedFilter} onChange={onFilterChange}>
                <option value="">Select Filter</option>
                <option value="fullName">Full Name</option>
                <option value="medicalRecordNumber">Medical Record Number</option>
                <option value="dateOfBirth">Date of Birth</option>
                <option value="email">Email</option>
                <option value="phoneNumber">Phone Number</option>
            </select>
            {selectedFilter && (
                <input
                    type={selectedFilter === 'dateOfBirth' ? 'date' : 'text'}
                    name={selectedFilter}
                    placeholder={selectedFilter.charAt(0).toUpperCase() + selectedFilter.slice(1).replace(/([A-Z])/g, ' $1')}
                    value={searchParams[selectedFilter]}
                    onChange={onSearchChange}
                />
            )}
        </div>
    );
}

export default PatientProfilesList;
