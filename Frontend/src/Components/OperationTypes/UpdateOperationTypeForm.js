import React, { useState, useEffect } from 'react';
import './AddOperationTypeForm.css';

// Componente para o Grid de Seleção do Staff
// eslint-disable-next-line no-unused-vars
const StaffSelectionGrid = ({ staffMembers, selectedStaff, onSelectionChange }) => {
    // Função para agrupar membros do staff por role
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
                    <h4>{role}</h4>
                    {groupedStaff[role].map((member) => (
                        <div key={member.id} className="staff-item">
                            <input
                                type="checkbox"
                                value={member.id}
                                checked={selectedStaff.includes(member.id)}
                                onChange={() => onSelectionChange(member.id)}
                            />
                            <span>{member.name}</span>
                        </div>
                    ))}
                </div>
            ))}
        </div>
    );
}

const UpdateOperationTypeForm = ({ operationType, staffMembers, onBack, onUpdate }) => {
    const [name, setName] = useState('');
    const [requiredStaff, setRequiredStaff] = useState([]);
    const [estimatedDuration, setEstimatedDuration] = useState('');
    const [error, setError] = useState(null);

    // Sincronizar os dados iniciais do formulário com o operationType recebido
    useEffect(() => {
        setName(operationType.name || '');
        setRequiredStaff(operationType.requiredStaff || []);
        setEstimatedDuration(operationType.estimatedDuration || '');
    }, [operationType]);

    const handleUpdate = (e) => {
        e.preventDefault();
        if (!name.trim() || !estimatedDuration) {
            setError('Name and Estimated Duration are required.');
            return;
        }
        setError(null);
        onUpdate(operationType.id, { name, requiredStaff, estimatedDuration: parseInt(estimatedDuration, 10) });
    };

    // eslint-disable-next-line no-unused-vars
    const handleStaffSelection = (id) => {
        setRequiredStaff((prevRequiredStaff) =>
            prevRequiredStaff.includes(id)
                ? prevRequiredStaff.filter((staffId) => staffId !== id)
                : [...prevRequiredStaff, id]
        );
    };

    return (
        <div className="add-operation-type-form-container">
            <h2>Update Operation Type</h2>
            {error && <p className="add-operation-type-form-error">{error}</p>}
            <form onSubmit={handleUpdate}>
                {/* Campo Nome */}
                <div className="add-operation-type-form-field">
                    <label className="add-operation-type-form-label">Name:</label>
                    <input
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        className="add-operation-type-form-input"
                        required
                    />
                </div>

                {/* Grid de Seleção do Staff */}
                <div className="add-operation-type-form-field">
                <label
                    htmlFor="operation-type-name"
                    className="add-operation-type-form-label"
                >
                    Name:
                </label>
                <input
                    id="operation-type-name"
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    className="add-operation-type-form-input"
                    required
                />
            </div>


                {/* Duração Estimada */}
                <div className="add-operation-type-form-field">
                    <label className="add-operation-type-form-label">Estimated Duration (min):</label>
                    <input
                        type="number"
                        value={estimatedDuration}
                        onChange={(e) => setEstimatedDuration(e.target.value)}
                        className="add-operation-type-form-input"
                        required
                        min="1"
                    />
                </div>

                {/* Botões de Ação */}
                <button type="submit" className="add-operation-type-form-button">Update</button>
            </form>
            <button onClick={onBack} className="add-operation-type-form-button" style={{ marginTop: '15px' }}>
                Back to List
            </button>
        </div>
    );
};

export default UpdateOperationTypeForm;
