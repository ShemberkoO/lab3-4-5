// AccidentsList.jsx
import React from 'react';
import AccidentCard from './AccidentCard';

const AccidentsInfo = ({ accidents, isLoading, error }) => {
    if (isLoading) {
        return <p>Loading...</p>;
    }

    if (error) {
        return <p>{error}</p>;
    }

    return (
        <div className="row">
            {accidents.map(accident => (
                <div className="col-12" key={accident.accidentId}>
                    <AccidentCard accident={accident} />
                </div>
            ))}
        </div>
    );
};

export default AccidentsInfo;
