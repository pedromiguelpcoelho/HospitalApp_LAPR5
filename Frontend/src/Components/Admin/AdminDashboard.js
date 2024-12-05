import React from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import './AdminDashboard.css';

const AdminDashboard = ({ user, logout }) => {
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/');
    };

    return (
        <div className="admin-dashboard">
            <aside className="sidebar">
                <h2>ADMIN MENU</h2>
                <nav>
                    <div className="section">
                        <h3>OPERATION TYPE</h3>
                        <ul>
                            <li><Link to="addoperationtype">Add Operation Type</Link></li>
                            <li><Link to="updateoperationtype">Update Operation Type</Link></li>
                            <li><Link to="deactivateoperationtype">Deactivate Operation Type</Link></li>
                            <li><Link to="getoperationtypes">List Operation Types</Link></li>
                        </ul>
                    </div>
                    <div className="section">
                        <h3>PATIENT PROFILE</h3>
                        <ul>
                            <li><Link to="addpatientprofile">Add Patient Profile</Link></li>
                            <li><Link to="updatepatientprofile">Update Patient Profile</Link></li>
                            <li><Link to="deletepatientprofile">Delete Patient Profile</Link></li>
                            <li><Link to="listpatientprofile">List Patient Profiles</Link></li>
                        </ul>
                    </div>
                    <div className="section">
                        <h3>STAFF PROFILE</h3>
                        <ul>
                            <li><Link to="addstaff">Add Staff</Link></li>
                            <li><Link to="updatestaff">Update Staff</Link></li>
                            <li><Link to="deactivatestaff">Delete Staff</Link></li>
                            <li><Link to="liststaff">List Staff</Link></li>

                        </ul>
                    </div>
                    <div className="section">
                        <h3>OPERATION REQUEST</h3>
                        <ul>
                            <li><Link to="search-operation-request-admin">Search Operation Request</Link></li>
                        </ul>
                    </div>
                </nav>
                <button onClick={handleLogout} className="logout-button">Logout</button>
            </aside>
            <main className="content">
                <Outlet />
            </main>
        </div>
    );
};

export default AdminDashboard;