import { useState, useEffect } from 'react';
import BaseModal from './BaseModal';
import axios from 'axios'; // Import axios
import 'bootstrap/dist/css/bootstrap.min.css';

export default function PeopleForm({ person, open, onClose }) {
    const [formData, setFormData] = useState({
        PasportId: '',
        FirstName: '',
        LastName: '',
        Patronymic: '',
        RegistrationAddress: '',
    });

    useEffect(() => {
        if (person) {
            setFormData({
                PasportId: person.pasportId || '',
                FirstName: person.firstName || '',
                LastName: person.lastName || '',
                Patronymic: person.patronymic || '',
                RegistrationAddress: person.registrationAddress || ''
            });
        } else {
            setFormData({
                PasportId: '',
                FirstName: '',
                LastName: '',
                Patronymic: '',
                RegistrationAddress: ''
            });
        }
    }, [person]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        if (person) {
            // Запит на редагування людини
            axios.put(`/api/Person/${person.pasportId}`, formData)
                .then(response => {
                    console.log('Людину успішно відредаговано:', response.data);
                    onClose();
                })
                .catch(error => {
                    console.error('Помилка при редагуванні:', error);
                });
        } else {
            // Запит на створення нової людини
            axios.post('/api/Person', formData)
                .then(response => {
                    console.log('Нову людину успішно створено:', response.data);
                    onClose();
                })
                .catch(error => {
                    console.error('Помилка при створенні:', error);
                });
        }
    };

    return (
        <BaseModal open={open} onClick={onClose}>
            <form onSubmit={handleSubmit}>
                <h5>{person ? 'Редагування людини' : 'Створення людини'}</h5>
                <div className="mb-3">
                    <label htmlFor="PasportId" className="form-label">Passport ID</label>
                    <input
                        type="text"
                        className="form-control"
                        id="PasportId"
                        name="PasportId"
                        value={formData.PasportId}
                        onChange={handleChange}
                        maxLength="20"
                        required
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="FirstName" className="form-label">First Name</label>
                    <input
                        type="text"
                        className="form-control"
                        id="FirstName"
                        name="FirstName"
                        value={formData.FirstName}
                        onChange={handleChange}
                        maxLength="50"
                        required
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="LastName" className="form-label">Last Name</label>
                    <input
                        type="text"
                        className="form-control"
                        id="LastName"
                        name="LastName"
                        value={formData.LastName}
                        onChange={handleChange}
                        maxLength="50"
                        required
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="Patronymic" className="form-label">Patronymic</label>
                    <input
                        type="text"
                        className="form-control"
                        id="Patronymic"
                        name="Patronymic"
                        value={formData.Patronymic}
                        onChange={handleChange}
                        maxLength="50"
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="RegistrationAddress" className="form-label">Registration Address</label>
                    <input
                        type="text"
                        className="form-control"
                        id="RegistrationAddress"
                        name="RegistrationAddress"
                        value={formData.RegistrationAddress}
                        onChange={handleChange}
                        maxLength="100"
                        required
                    />
                </div>
                <div className="d-flex justify-content-between">
                    <button type="button" className="btn btn-secondary" onClick={onClose}>Закрити</button>
                    <button type="submit" className="btn btn-primary">Зберегти</button>
                </div>
            </form>
        </BaseModal>
    );
}
