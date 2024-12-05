import React, { useState } from 'react';
import { Link } from 'react-router-dom';

const SignUpForm = ({ onSignup }) => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [name, setName] = useState('');
    const [acceptTerms, setAcceptTerms] = useState(false);

    const handleSubmit = (e) => {
        e.preventDefault();
        if (!acceptTerms) {
            alert('You must accept the terms and conditions to sign up.');
            return;
        }
        onSignup(email, password, name);
    };

    return (
        <div className="form-container">
            <h2>SIGN UP</h2>
            <form onSubmit={handleSubmit}>
                <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    placeholder="Email"
                    required
                />
                <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Password"
                    required
                />
                <input
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    placeholder="Name"
                    required
                />
                <div className="checkbox-container">
                    <input
                        type="checkbox"
                        id="acceptTerms"
                        checked={acceptTerms}
                        onChange={(e) => setAcceptTerms(e.target.checked)}
                        required
                    />
                    <label htmlFor="acceptTerms">
                        I agree to the <Link to="/terms" target="_blank" rel="noopener noreferrer">terms and conditions</Link>.
                    </label>
                </div>
                <button type="submit">Register</button>
            </form>
        </div>
    );
};

export default SignUpForm;
