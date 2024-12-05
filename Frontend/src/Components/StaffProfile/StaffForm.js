import React, { useState, useEffect } from 'react';


function StaffForm({ onCreateStaffMember, staffData, handleInputChange }) {
    const [specializationOptions, setSpecializationOptions] = useState([]);


    useEffect(() => {
        if (staffData.role === 'Doctor') {
            setSpecializationOptions(['Orthopaedist', 'Anaesthetist']);
        } else if (staffData.role === 'Nurse') {
            setSpecializationOptions(['Instrumenting Nurse', 'Circulating Nurse', 'Nurse Anaesthetist', 'Medical Action Assistant']);
        } else if (staffData.role === 'Other') {
            setSpecializationOptions(['X-ray Technician']);
        } else {
            setSpecializationOptions([]);  
        }
    }, [staffData.role]);

    const handleSubmit = (e) => {
        e.preventDefault();

        if (!staffData.firstName || !staffData.lastName || !staffData.email || !staffData.phoneNumber || !staffData.role || !staffData.specialization) {
            return; 
        }

        
        onCreateStaffMember(staffData);
    };

    return (
        <div id="staff-form">
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    name="firstName"
                    value={staffData.firstName}
                    onChange={handleInputChange}
                    placeholder="First Name"
                    required
                />
                <input
                    type="text"
                    name="lastName"
                    value={staffData.lastName}
                    onChange={handleInputChange}
                    placeholder="Last Name"
                    required
                />
                <input
                    type="email"
                    name="email"
                    value={staffData.email}
                    onChange={handleInputChange}
                    placeholder="Email"
                    required
                />
                <input
                    type="text"
                    name="phoneNumber"
                    value={staffData.phoneNumber}
                    onChange={handleInputChange}
                    placeholder="Phone Number"
                    required
                />
               <label htmlFor="role">Role</label>
                <select
                    id="role"
                    name="role"
                    value={staffData.role}
                    onChange={handleInputChange}
                    required
                >
                    <option value="">Select Role</option>
                    <option value="Doctor">Doctor</option>
                    <option value="Nurse">Nurse</option>
                    <option value="Other">Other</option>
                </select>

               {}
                {staffData.role && (
                <>
                    <label htmlFor="specialization">Specialization</label>
                    <select
                    id="specialization"
                    name="specialization"
                    value={staffData.specialization}
                    onChange={handleInputChange}
                    required
                    >
                    <option value="">Select Specialization</option>
                    {specializationOptions.map((option, index) => (
                        <option key={index} value={option}>
                        {option}
                        </option>
                    ))}
                    </select>
                </>
                )}

                <button type="submit">
                    Create
                </button>
            </form>
        </div>
    );
}

export default StaffForm;

