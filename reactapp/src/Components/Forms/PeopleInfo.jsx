// PeopleList.jsx
import React from 'react';
import Person from './Person'; // Імпортуємо компонент Person

const PeopleInfo = ({ people, isLoading, error }) => {
    if (isLoading) {
        return <p>Loading...</p>;
    }

    if (error) {
        return <p>{error}</p>;
    }

    return (
        <div className="row">
            {people.map(person => (
                <div className="col-12" key={person.pasportId}>
                    <Person person={person} />
                </div>
            ))}
        </div>
    );
};

export default PeopleInfo;
