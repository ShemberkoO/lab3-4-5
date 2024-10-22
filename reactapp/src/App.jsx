import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Navbar from './components/Navbar';
import routes from './routes'

const App = () => {
    return (
        <Router>
            <Navbar />
            <div className="container-fluid mt-5 pt-5" style={{ height: '100vh' }}>
                <Routes>
                    {routes.map((route, index) => (
                        <Route key={index} path={route.path} element={route.element} />
                    ))}
                </Routes>
            </div>
        </Router>
    );
};

export default App;
