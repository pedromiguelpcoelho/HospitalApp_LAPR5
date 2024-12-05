import React, { useState, useEffect } from 'react';
import './AddOperationTypeForm.css';

// Componente para o Grid de Seleção do Staff
const StaffSelectionGrid = ({ staffMembers, selectedStaff, onSelectionChange }) => {
    const groupByRole = (staff) => {
        return staff.reduce((acc, member) => {
            acc[member.role] = acc[member.role] || [];
            acc[member.role].push(member);
            return acc;
        }, {});
    };

    const groupedStaff = groupByRole(staffMembers);

    return (
        <div className="staff-grid-container">
            {Object.keys(groupedStaff).map((role) => (
                <div key={role} className="staff-column">
                    <h4 className="staff-column-title">{role}</h4>
                    {groupedStaff[role].map((member) => (
                        <div key={member.id} className="staff-item">
                            <input
                                type="checkbox"
                                id={`staff-${member.id}`}
                                value={member.id}
                                checked={selectedStaff.includes(member.id)}
                                onChange={() => onSelectionChange(member.id)}
                            />
                            <label htmlFor={`staff-${member.id}`}>{member.name}</label>
                        </div>
                    ))}
                </div>
            ))}
        </div>
    );
};

const OperationTypeForm = ({ staffMembers, onCreate, initialData }) => {
    const [formData, setFormData] = useState({
        name: initialData?.name || '',
        requiredStaff: initialData?.requiredStaff || [],
        estimatedDuration: initialData?.estimatedDuration || '',
    });

    const [errors, setErrors] = useState({});
    const { name, requiredStaff, estimatedDuration } = formData;

    useEffect(() => {
        if (initialData) {
            setFormData({
                name: initialData.name || '',
                requiredStaff: initialData.requiredStaff || [],
                estimatedDuration: initialData.estimatedDuration || '',
            });
        }
    }, [initialData]); // Atualiza o formulário sempre que `initialData` mudar

    const validate = () => {
        const newErrors = {};
        if (!name.trim()) newErrors.name = 'Name is required.';
        if (requiredStaff.length === 0) newErrors.requiredStaff = 'At least one staff member must be selected.';
        if (!estimatedDuration || isNaN(estimatedDuration) || estimatedDuration <= 0)
            newErrors.estimatedDuration = 'Estimated duration must be a positive number.';
        return newErrors;
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prevData => ({
            ...prevData,
            [name]: value
        }));
    };

    const handleStaffSelection = (id) => {
        setFormData(prevData => ({
            ...prevData,
            requiredStaff: prevData.requiredStaff.includes(id)
                ? prevData.requiredStaff.filter(staffId => staffId !== id)
                : [...prevData.requiredStaff, id]
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        const validationErrors = validate();
        if (Object.keys(validationErrors).length > 0) {
            setErrors(validationErrors);
            return;
        }
        onCreate(formData);
    };

    return (
        <div className="add-operation-type-form-container">
            <h2>{initialData?.name ? 'Update' : 'Add'} Operation Type</h2>
            <form onSubmit={handleSubmit}>
                {/* Campo de Nome */}
                <div className="add-operation-type-form-field">
                    <label className="add-operation-type-form-label" htmlFor="name">Name:</label>
                    <input
                        id="name"
                        type="text"
                        name="name"
                        value={name}
                        onChange={handleChange}
                        className={`add-operation-type-form-input ${errors.name ? 'error' : ''}`}
                    />
                    {errors.name && <span className="error-message">{errors.name}</span>}
                </div>

                {/* Campo de Staff */}
                <div className="add-operation-type-form-field">
                    <label className="add-operation-type-form-label">Required Staff:</label>
                    <StaffSelectionGrid
                        staffMembers={staffMembers}
                        selectedStaff={requiredStaff}
                        onSelectionChange={handleStaffSelection}
                    />
                    {errors.requiredStaff && <span className="error-message">{errors.requiredStaff}</span>}
                </div>

                {/* Campo de Duração Estimada */}
                <div className="add-operation-type-form-field">
                    <label className="add-operation-type-form-label" htmlFor="estimatedDuration">
                        Estimated Duration (min):
                    </label>
                    <input
                        id="estimatedDuration"
                        type="number"
                        name="estimatedDuration"
                        value={estimatedDuration}
                        onChange={handleChange}
                        className={`add-operation-type-form-input ${errors.estimatedDuration ? 'error' : ''}`}
                        min="1"
                    />
                    {errors.estimatedDuration && <span className="error-message">{errors.estimatedDuration}</span>}
                </div>

                {/* Botão de Submit */}
                <button type="submit" className="add-operation-type-form-button">
                    {initialData?.name ? 'Update' : 'Add'} Operation Type
                </button>
            </form>
        </div>
    );
};

export default OperationTypeForm;
