import React from 'react';
import SignUpForm from '../../Components/Auth/SignUpForm';
import fetchApi from '../../Services/fetchApi';

const SignUpPage = () => {
    const handleSignup = async (email, password, name) => {
        try {
            const data = await fetchApi('/auth/signup', {
                method: 'POST',
                body: JSON.stringify({ email, password, name }),
            });

            alert(data.Message);
        } catch (err) {
            alert(err.message || JSON.stringify(err));
        }
    };

    return <SignUpForm onSignup={handleSignup} />;
};

export default SignUpPage;