import React from 'react';

function UpdateStaffForm({
    onFormChange,
    onUpdate,
    formData,
    loading
}) {
    return (
        <div>
            {}
            {formData && (
                <form>
                    <input
                        type="text"
                        name="firstName"
                        placeholder="First Name"
                        value={formData.firstName || ''}
                        onChange={onFormChange}
                    />
                    <input
                        type="text"
                        name="lastName"
                        placeholder="Last Name"
                        value={formData.lastName || ''}
                        onChange={onFormChange}
                    />
                    <input
                        type="email"
                        name="email"
                        placeholder="Email"
                        value={formData.email || ''}
                        onChange={onFormChange}
                    />
                    <input
                        type="text"
                        name="specialization"
                        placeholder="Specialization"
                        value={formData.specialization || ''}
                        onChange={onFormChange}
                    />
                    <input
                        type="text"
                        name="role"
                        placeholder="Role"
                        value={formData.role || ''}
                        onChange={onFormChange}
                    />
                    <input
                        type="text"
                        name="phoneNumber"
                        placeholder="Phone Number"
                        value={formData.phoneNumber || ''}
                        onChange={onFormChange}
                    />
                    <button type="button" onClick={onUpdate} disabled={loading}>Update</button>
                </form>
            )}
        </div>
    );
}

export default UpdateStaffForm;