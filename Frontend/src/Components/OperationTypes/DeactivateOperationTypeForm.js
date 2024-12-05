import React, { useState } from 'react';

const DeactivateOperationTypeForm = ({ operationType, onDeactivateType }) => {
    const [error, setError] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        setError(null);

        // Call the onDeactivateType function passed from the parent
        onDeactivateType(operationType.id);
    };

    return (
        <div id="deactivate-type-form">
            <h2>Deactivate Operation Type</h2>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            <form onSubmit={handleSubmit}>
                <p>Are you sure you want to deactivate the operation type: {operationType.name}?</p>
                <button type="submit">Deactivate</button>
            </form>
        </div>
    );
};

export default DeactivateOperationTypeForm;