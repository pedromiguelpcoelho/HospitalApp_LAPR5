import React from 'react';

function UpdatePatientProfileForm({
    searchParams,
    selectedFilter,
    onSearchChange,
    onFilterChange,
    onFormChange,
    onSearch,
    onUpdate,
    formData,
    loading
}) {
    return (
        <div>
            <select value={selectedFilter} onChange={onFilterChange}>
                <option value="">Select Filter</option>
                <option value="email">Email</option>
                <option value="medicalRecordNumber">Medical Record Number</option>
                <option value="fullName">Full Name</option>
                <option value="dateOfBirth">Date of Birth</option>
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
            <button onClick={onSearch} disabled={loading}>Search</button>

            {formData && (
                <form>
                    <input
                        type="text"
                        name="firstName"
                        placeholder="First Name"
                        value={formData.firstName}
                        onChange={onFormChange}
                    />
                    <input
                        type="text"
                        name="lastName"
                        placeholder="Last Name"
                        value={formData.lastName}
                        onChange={onFormChange}
                    />
                    <input
                        type="text"
                        name="fullName"
                        placeholder="Full Name"
                        value={formData.fullName}
                        onChange={onFormChange}
                    />
                    <input
                        type="email"
                        name="email"
                        placeholder="Email"
                        value={formData.email}
                        onChange={onFormChange}
                    />
                    <input
                        type="text"
                        name="contactInformation"
                        placeholder="Contact Information"
                        value={formData.contactInformation}
                        onChange={onFormChange}
                    />
                    <input
                        type="date"
                        name="dateOfBirth"
                        placeholder="Date of Birth"
                        value={formData.dateOfBirth}
                        onChange={onFormChange}
                    />
                    <input
                        type="text"
                        name="medicalRecordNumber"
                        placeholder="Medical Record Number"
                        value={formData.medicalRecordNumber}
                        onChange={onFormChange}
                    />
                    <button type="button" onClick={onUpdate} disabled={loading}>Update</button>
                </form>
            )}
        </div>
    );
}

export default UpdatePatientProfileForm;