import React, { useState, useEffect } from 'react';
import axios from 'axios';
import AccidentsInfo from '../../Components/Accident/AccidentsInfo'; // Компонент для відображення списку
import AccidentsForm from '../../Components/Forms/AccidentsForm'; // Форма для створення
import DeleteAccidentsBeforeDateForm from '../../Components/Forms/DeleteAccidentsBeforeDateForm';

const Accidents = () => {
    const [accidents, setAccidents] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const [isOpen, setIsOpen] = useState(false);

    const [isOpenDelete, setIsOpenDelete] = useState(false);

    const [selectedYear, setSelectedYear] = useState(null); // Стан для вибраного року
    const [searchQuery, setSearchQuery] = useState(''); // Стан для вводу тексту
    const [searchTrigger, setSearchTrigger] = useState(''); // Стан для виконання пошуку

    const handleOpenModal = () => {
        setIsOpen(true);
    };

    const handleOpenDeleteModal = () => {
        setIsOpenDelete(true);
    };

    

    const handleCloseModal = () => {
        setIsOpen(false);
        setIsOpenDelete(false);
        fetchAccidents();
    };

    const fetchAccidents = (year = null, query = '') => {
        setIsLoading(true);
        let url = 'api/Accidents';

        const params = new URLSearchParams();
        if (year) params.append('year', year);
        if (query) params.append('query', query);

        axios
            .get(`${url}?${params.toString()}`)
            .then((response) => {
                setAccidents(response.data);
                setIsLoading(false);
            })
            .catch((err) => {
                setError('Error fetching accidents.');
                setIsLoading(false);
            });
    };

    useEffect(() => {
        fetchAccidents(selectedYear, searchTrigger); // Пошук активується при зміні `searchTrigger`
    }, [selectedYear, searchTrigger]);

    const handleYearFilter = (year) => {
        setSelectedYear(year);
    };

    const handleSearch = () => {
        setSearchTrigger(searchQuery); // Запуск пошуку за введеним запитом
    };

    return (
        <div className="container-fluid d-flex flex-column justify-content-center">
            <div className="d-flex flex-row justify-content-between align-items-center">
                <h1>Accidents</h1>
                <div>
                    {/* Кнопки для фільтрації за роками */}
                    <button
                        className={`btn btn-primary mx-1 ${selectedYear === 2022 ? 'active' : ''}`}
                        onClick={() => handleYearFilter(2022)}
                    >
                        2022
                    </button>
                    <button
                        className={`btn btn-primary mx-1 ${selectedYear === 2023 ? 'active' : ''}`}
                        onClick={() => handleYearFilter(2023)}
                    >
                        2023
                    </button>
                    <button
                        className={`btn btn-primary mx-1 ${selectedYear === 2024 ? 'active' : ''}`}
                        onClick={() => handleYearFilter(2024)}
                    >
                        2024
                    </button>
                    <button
                        className="btn btn-secondary mx-1"
                        onClick={() => handleYearFilter(null)}
                    >
                        All
                    </button>
                </div>
                <button className="btn btn-secondary" onClick={handleOpenModal}>
                    Create
                </button>
                <button className="btn btn-secondary" onClick={handleOpenDeleteModal}>
                    Delete
                </button>
                <AccidentsForm open={isOpen} onClose={handleCloseModal} />
                <DeleteAccidentsBeforeDateForm open={isOpenDelete} onClose={handleCloseModal} />
            </div>

            {/* Поле пошуку */}
            <div className="my-3 d-flex">
                <input
                    type="text"
                    className="form-control me-2"
                    placeholder="Search by location or description..."
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                />
                <button className="btn btn-primary" onClick={handleSearch}>
                    Search
                </button>
            </div>

            <AccidentsInfo accidents={accidents} isLoading={isLoading} error={error} />
        </div>
    );
};

export default Accidents;
