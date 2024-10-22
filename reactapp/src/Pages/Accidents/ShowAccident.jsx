import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams } from 'react-router-dom';
import ViolationsInfo from '../../Components/Violations/ViolationsInfo';
import PeopleInfo from '../../Components/People/PeopleInfo';
import AccidentsForm from '../../Components/Forms/AccidentsForm'

import { useNavigate } from 'react-router-dom';
const ShowAccident = () => {
    const navigate = useNavigate();
    const [people, setPeople] = useState([]);
    const [accident, setAccident] = useState({});
    const [violations, setViolations] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const params = useParams();

    const [isOpen, setOpen] = useState(false);

    const handleOpenModal = () => {
        setOpen(true);
    }

    const handleCloseModal = () => {
        getInfo();
        setOpen(false);
    }

    const getInfo = () => {
        axios.get(`https://localhost:7217/api/Accidents/${params.accidentId}/show`)
            .then(response => {
                console.log(response.data);
                setAccident(response.data);
                setPeople(response.data.people || []);
                setViolations(response.data.violations || []);
                setIsLoading(false);
            })
            .catch(err => {
                setError('Error fetching people.');
                setIsLoading(false);
            });
    }
    

    useEffect(() => {
        getInfo();
    }, [params.personId]);


    const handleDelete = () => {
        if (window.confirm('Are you sure you want to delete this person?')) {
            axios.delete(`https://localhost:7217/api/Accidents/${params.accidentId}`)
                .then(() => {
                    navigate('/accidents');
                })
                .catch(err => {
                    alert(`Error deleting person. ${err}`);
                    navigate('/accidents');
                });
        }
    };



    return (
        <div className="container-fluid d-flex flex-column justify-content-center">
            <div className="row">
                <div className="col-md-10 mx-auto">
                    {/* Інформація про людину з тінню та бордером */}
                    <div className="card shadow-lg border-0 p-4 mb-4">
                        <div className="d-flex justify-content-between mb-3">
                            <div>
                                <h5 className="card-title">Location:{accident.location}</h5>
                                <p className="card-text">Date:{accident.date}</p>
                            </div>
                            <div>
                                <h3 className="card-title">Id: {accident.accidentId}</h3>
                            </div>
                        </div>
                        <div className="d-flex justify-content-between">
                            <p className="card-text">{accident.description}</p>
                            <div className='d-flex'>
                                <button className="btn btn-danger btn-sm" onClick={handleDelete}>
                                    <i className="fas fa-trash-alt"></i> Delete
                                </button>
                                <button className="btn btn-secondary mx-2" onClick={() => handleOpenModal()}>
                                    Edit
                                </button>
                            </div>
                        

                            <AccidentsForm open={isOpen} accident={accident} onClose={handleCloseModal} />
                        </div>
                    </div>
                </div>
            </div>
            <div className="row">
                {/* Перевірка на порожність масивів перед рендерингом */}

                    <div className="col-md-6">
                        {/* Блок з інфо про нещасні випадки */}
                        <div className="bg-light p-3 shadow-sm rounded mb-3" style={{ height: '60vh', overflowY: 'auto', backgroundColor: '#f8f9fa' }}>
                            <h5>People</h5>
                        {people.length > 0 && < PeopleInfo people={people} isLoading={isLoading} error={error} />}
                        </div>
                    </div>
                
                 
                    <div className="col-md-6">
                        {/* Блок з інфо про порушення */}
                        <div className="bg-light p-3 shadow-sm rounded mb-3" style={{ height: '60vh', overflowY: 'auto', backgroundColor: '#f1f3f5' }}>
                            <h5>Violations</h5>
                        {violations.length > 0 && <ViolationsInfo violations={violations} isLoading={isLoading} error={error} />}
                        </div>
                    </div>
                
            </div>
        </div>
    );
};

export default ShowAccident;
