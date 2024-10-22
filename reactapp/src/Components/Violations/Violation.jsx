// PersonCard.jsx
import { Button } from 'bootstrap';
import React from 'react';
import { Link } from 'react-router-dom'

const Violation = ({ violation }) => {
    console.log(violation)
    return (
        <div className="card mb-3">
            <div className="card-body">
                <div className="d-flex justify-content-between ">
                    <div>
                        <h5 className="card-title">{violation.article}</h5>
                        <p className="card-text">VictimId: {violation.victimId}</p>
                    </div>
                    <div>
                        <h3 className="card-title">Id: {violation.violationId} </h3>
                    </div>
                </div>


                <div className="d-flex justify-content-between">
                    <p className="card-text">Comment: {violation.comment}</p>
                </div>
            </div>
        </div>
    );
};

export default Violation;
