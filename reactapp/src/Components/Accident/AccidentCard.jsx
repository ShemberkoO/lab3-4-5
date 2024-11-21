// PersonCard.jsx
import { Button } from 'bootstrap';
import {React} from 'react';
import { Link } from 'react-router-dom'

const AccidentCard = ({ accident }) => {
    return (
        <div className="card mb-3">
            <div className="card-body">
                <div className="d-flex justify-content-between ">
                    <div>
                        <h4>{accident.date}</h4>
                        <h5>{accident.location}</h5>
                    </div>
                    <div>
                        <h3 className="card-title">Id: {accident.accidentId} </h3>
                        <Link className="nav-link" to={`/accidents/${accident.accidentId}`}>Show</Link>
                        <Link className="nav-link" to={`/accidents/${accident.accidentId}`}></Link>
                    </div>
                </div>


                <div className="d-flex justify-content-between">
                    <a>{accident.description}</a>
                   
                </div>
            </div>
        </div>
    );
};

export default AccidentCard;
