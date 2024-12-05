import React from 'react';

const OperationTypeDetailsPage = ({ operationType, staffMembers, onBack }) => {
    const getStaffNames = (staffIds) => {
        if (!staffIds || staffIds.length === 0) return 'Unknown';
        return staffIds
            .map((id) => {
                const staff = staffMembers.find((member) => member.id === id);
                return staff ? staff.name : 'Unknown';
            })
            .join(', ');
    };

    return (
        <div>
            <h2>Operation Type Details</h2>
            {operationType ? (
                <div>
                    <p>
                        <strong>Name:</strong> {operationType.name}
                    </p>
                    <p>
                        <strong>Staff:</strong> {getStaffNames(operationType.requiredStaff)}
                    </p>
                    <p>
                        <strong>Estimated Duration:</strong> {operationType.estimatedDuration} min
                    </p>
                </div>
            ) : (
                <p>No operation type selected.</p>
            )}
        </div>
    );
};

export default OperationTypeDetailsPage;
