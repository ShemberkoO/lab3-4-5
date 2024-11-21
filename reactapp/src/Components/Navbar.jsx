import React from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';

const Navbar = () => {
    const handleCommit = async () => {
        try {
            const response = await axios.post('/api/Accidents/commit'); 
            alert(`Commit : ${response.data}`); 
            console.log(response)
        } catch (error) {
            console.error('Commit failed:', error);
            alert('Commit failed. Please try again.');
        }
    };

    return (
        <header>
            <nav className="navbar navbar-expand-lg navbar-light bg-light fixed-top">
                <div className="container-fluid">
                    <Link className="navbar-brand" to="/">Main Page</Link>
                    <button
                        className="navbar-toggler"
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target="#navbarNav"
                        aria-controls="navbarNav"
                        aria-expanded="false"
                        aria-label="Toggle navigation"
                    >
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarNav">
                        <ul className="navbar-nav">
                            <li className="nav-item">
                                <Link className="nav-link" to="/">Home</Link>
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link" to="/accidents">Accidents</Link>
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link" to="/people">People</Link>
                            </li>
                        </ul>
                        <button
                            className="btn btn-primary ms-auto"
                            onClick={handleCommit}
                        >
                            Commit
                        </button>
                    </div>
                </div>
            </nav>
        </header>
    );
};

export default Navbar;
