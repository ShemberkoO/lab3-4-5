// PeopleList.jsx
import React from 'react';
import Violation from './Violation'; // Імпортуємо компонент Person

const ViolationsInfo = ({ violations, isLoading, error }) => {
    if (isLoading) {
        return <p>Loading...</p>;
    }

    if (error) {
        return <p>{error}</p>;
    }

    return (
        <div className="row">
            {violations.map(violation => (
                <div className="col-12" key={violation.violationId}>
                    <Violation violation={violation} />
                </div>
            ))}
        </div>
    );
};

export default ViolationsInfo;
