import React from 'react';
import './TermsAndConditions.css';

const TermsAndConditions = () => {
    const handleClose = () => {
        window.close();
    };

    return (
        <div className="terms-container">
            <h1 className="terms-title">Terms and Conditions</h1>
            <iframe 
                src="/Privacy%20Policy.pdf" 
                className="terms-iframe"
                title="Terms and Conditions" 
            />
            <button className="close-button" onClick={handleClose}>Back to SignUp</button>
        </div>
    )
};

export default TermsAndConditions;