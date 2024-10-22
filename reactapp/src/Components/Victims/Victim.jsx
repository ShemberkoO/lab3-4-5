// PersonCard.jsx
import { Button } from 'bootstrap';
import React from 'react';
import { Link } from 'react-router-dom'

const Victim = ({ victim }) => {
    console.log(victim)
    return (
        <div className="card mb-3">
            <div className="card-body">
                <div className="d-flex justify-content-between ">
                    <div>
                        <h5 className="card-title">{person.firstName} {person.lastName}</h5>
                        <p className="card-text">Patronymic: {person.patronymic}</p>
                    </div>
                    <div>
                        <h3 className="card-title">PassportId: {person.pasportId} </h3>
                    </div>
                </div>


                <div className="d-flex justify-content-between">
                    <p className="card-text">Registration Address: {person.registrationAddress}</p>
                </div>
            </div>
        </div>
    );
};

export default Victim;
