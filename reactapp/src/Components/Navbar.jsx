import React from 'react';
import { Link } from 'react-router-dom';
const Navbar = () =>{
    return (<header>
        <nav className="navbar navbar-expand-lg navbar-light bg-light fixed-top">
            <div className="container-fluid">
                <Link className="navbar-brand" to="/">Main Page</Link>
                <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
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
                </div>
            </div>
        </nav>
    </header>);
}

export default Navbar;