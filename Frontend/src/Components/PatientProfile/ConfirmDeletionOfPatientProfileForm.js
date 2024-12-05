import React, { useState } from 'react';

const ConfirmDeletionOfPatientProfileForm = ({ handleSubmit }) => {
    const [token, setToken] = useState('');

    const onSubmit = (e) => {
        e.preventDefault();
        handleSubmit(token);
    };

    return (
        <div>
            <h2>Confirm Account Deletion</h2>
            <form onSubmit={onSubmit}>
                <label>
                    Confirmation Token:
                    <input
                        type="text"
                        value={token}
                        onChange={(e) => setToken(e.target.value)}
                        required
                    />
                </label>
                <button type="submit">Confirm Deletion</button>
            </form>
        </div>
    );
};

export default ConfirmDeletionOfPatientProfileForm;