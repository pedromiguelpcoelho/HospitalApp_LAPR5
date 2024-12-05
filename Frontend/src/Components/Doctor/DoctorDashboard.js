import React from 'react';
import { useNavigate, Link, Outlet } from 'react-router-dom';
import './DoctorDashboard.css';

const DoctorDashboard = ({ user, logout }) => {
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/');
    };

    return (
        <div className="doctor-dashboard-container">
            <aside className="doctor-sidebar">
                <h2>DOCTOR MENU</h2>
                <nav className="doctor-sidebar-nav">
                    <ul>
                        <li><Link to="add-request-operation" className="doctor-nav-item">Add Operation Request</Link></li>
                        <li><Link to="update-request-operation" className="doctor-nav-item">Update Operation Request</Link></li>
                        <li><Link to="delete-request-operation" className="doctor-nav-item">Delete Operation Request</Link></li>
                        <li><Link to="search-request-operation" className="doctor-nav-item">Search Operation Requests</Link></li>
                    </ul>
                </nav>
                <h2>3D HOSPITAL FLOOR</h2>
                <nav className="doctor-sidebar-nav">
                    <ul>
                        <li><Link to="hospital-floor" className="doctor-nav-item">View Hospital Floor</Link></li>
                    </ul>
                </nav>    
                <button className="doctor-logout-button" onClick={handleLogout}>Logout</button>
            </aside>
            <main className="doctor-main-content">
                <Outlet />
            </main>
        </div>
    );
};

export default DoctorDashboard;