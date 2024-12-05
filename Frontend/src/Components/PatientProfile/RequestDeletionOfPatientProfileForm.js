import React from 'react';

const RequestDeletionOfPatientProfileForm = ({ handleSubmit }) => {
    return (
        <div>
            <h2>Request Account Deletion</h2>
            <form onSubmit={handleSubmit}>
                <button type="submit">Request Deletion</button>
            </form>
        </div>
    );
};

export default RequestDeletionOfPatientProfileForm;