// People.jsx
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import PeopleInfo from '../../Components/People/PeopleInfo';
import PeopleForm from '../../Components/Forms/PeopleForm'

const People = () => {
    const [people, setPeople] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const [isOpen, setIsOpen] = useState(false);

    const handleOpenModal = () => {
        setIsOpen(true);
    }

    const handleCloseModal = () => {
        setIsOpen(false);
        fetchPeople();
    };


    const fetchPeople = () => {
        setIsLoading(true);
        axios.get('api/Person')
            .then(response => {
                setPeople(response.data);
                setIsLoading(false);
            })
            .catch(err => {
                setError('Error fetching people.');
                setIsLoading(false);
            });
    };

    
    useEffect(() => {
        fetchPeople();
    }, []);

    return (
        <div className="container-fluid d-flex flex-column justify-content-center">
            <div className='d-flex flex-row justify-content-between'>
                
                <h1>People</h1>
                <button className="btn btn-secondary" onClick={() => handleOpenModal()}>
                   Create
                </button>

                <PeopleForm open={isOpen}  onClose={handleCloseModal}/>
            </div>
            
            <PeopleInfo people={people} isLoading={isLoading} error={error} />
        </div>
    );
};

export default People;
