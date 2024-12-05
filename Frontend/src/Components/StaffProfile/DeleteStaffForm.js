import React, { useState } from 'react';


const DeleteStaffForm = ({ staffList, onDelete }) => {
    const [selectedStaffId, setSelectedStaffId] = useState('');
    const [selectedStaffDetails, setSelectedStaffDetails] = useState(null);

    const handleSelectChange = (e) => {
        const staffId = e.target.value;
        setSelectedStaffId(staffId);
        const selectedStaff = staffList.find(staff => staff.id === staffId);
        setSelectedStaffDetails(selectedStaff);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onDelete(selectedStaffId);
    };

    return (
        <form onSubmit={handleSubmit} className="delete-staff-form-container">
            <div className="delete-staff-form-field">
                <label className="delete-staff-form-label">
                    Select Staff ID:
                </label>
                <select
                    value={selectedStaffId}
                    onChange={handleSelectChange}
                    required
                    className="delete-staff-form-select"
                >
                    <option value="">Select a Staff ID</option>
                    {staffList.map((staff) => (
                        <option key={staff.id} value={staff.id}>
                            {staff.id + ' - ' + staff.name}
                        </option>
                    ))}
                </select>
            </div>
            {selectedStaffDetails && (
                <div className="delete-staff-form-details">
                    <h3>Staff Details</h3>
                    <p><strong>ID:</strong> {selectedStaffDetails.id}</p>
                    <p><strong>Full Name:</strong> {selectedStaffDetails.name}</p>
                    <p><strong>Email:</strong> {selectedStaffDetails.email}</p>
                    <p><strong>Phone Number:</strong> {selectedStaffDetails.phoneNumber}</p>
                    <p><strong>Specialization:</strong> {selectedStaffDetails.specialization}</p>
                    <p><strong>License Number:</strong> {selectedStaffDetails.licenseNumber}</p>
                    <p><strong>Status:</strong> {selectedStaffDetails.isActive ? 'Active' : 'Inactive'}</p>
                </div>
            )}
            <button type="submit" className="delete-staff-form-button">
                Deactivate
            </button>
        </form>
    );
};

export default DeleteStaffForm;