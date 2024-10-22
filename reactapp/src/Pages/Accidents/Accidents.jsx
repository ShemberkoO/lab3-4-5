// Accidents.jsx
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import AccidentsInfo from '../../Components/Accident/AccidentsInfo';// Імпортуємо новий компонент
import AccidentsForm from '../../Components/Forms/AccidentsForm'


const Accidents = () => {
    const [accidents, setAccidents] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);

    const [isOpen, setIsOpen] = useState(false);

    const handleOpenModal = () => {
        setIsOpen(true);
    }

    const handleCloseModal = () => {
        setIsOpen(false);
        fetchAccidents();
    };

    const fetchAccidents = () => {
        axios.get('api/Accidents')
            .then(response => {
                setAccidents(response.data);
                setIsLoading(false);
            })
            .catch(err => {
                setError('Error fetching accidents.');
                setIsLoading(false);
            });
    }
    useEffect(() => {
        fetchAccidents();
    }, []);

    return (
        <div className="container-fluid d-flex flex-column justify-content-center">
            <div className='d-flex flex-row justify-content-between'>

                <h1>Accidents</h1>
                <button className="btn btn-secondary" onClick={() => handleOpenModal()}>
                    Create
                </button>

                <AccidentsForm open={isOpen} onClose={handleCloseModal} />
            </div>
            <AccidentsInfo accidents={accidents} isLoading={isLoading} error={error} />
        </div>
    );
};

export default Accidents;
