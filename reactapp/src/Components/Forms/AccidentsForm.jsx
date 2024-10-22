import { useState, useEffect } from 'react';
import BaseModal from './BaseModal';
import axios from 'axios'; // Import axios
import 'bootstrap/dist/css/bootstrap.min.css';

export default function AccidentsForm({ accident, open, onClose }) {
    const [formData, setFormData] = useState({
        Date: '',
        Location: '',
        Description: '',
    });

    useEffect(() => {
        if (accident) {
            setFormData({
                Date: accident.date || '',
                Location: accident.location || '',
                Description: accident.description || '',
            });
        } else {
            setFormData({
                Date: '',
                Location: '',
                Description: '',
            });
        }
    }, [accident]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        if (accident) {
            axios.put(`/api/Accidents/${accident.accidentId}`, { ...formData, AccidentId: accident.accidentId })
                .then(response => {
                    console.log('Інцидент успішно відредаговано:', response.data);
                    onClose();
                })
                .catch(error => {
                    console.error('Помилка при редагуванні інциденту:', error);
                    if (error.response && error.response.status === 400) {
                        // Отримуємо список помилок з відповіді сервера
                        const errorMessages = error.response.data.errors;

                        // Формуємо одне повідомлення з усіх помилок
                        let fullErrorMessage = '';
                        for (const field in errorMessages) {
                            if (errorMessages.hasOwnProperty(field)) {
                                fullErrorMessage += `${field}: ${errorMessages[field].join(', ')}\n`;
                            }
                        }

                        // Виводимо повідомлення користувачу (наприклад, через alert або у ваш UI)
                        alert(fullErrorMessage);
                    }
                    onClose();
                });
        } else {
            axios.post('/api/Accidents', formData)
                .then(response => {
                    console.log('Новий інцидент успішно створено:', response.data);
                    onClose();
                })
                .catch(error => {
                    console.error('Помилка при створенні інциденту:', error.response);
                    if (error.response && error.response.status === 400) {
                        // Отримуємо список помилок з відповіді сервера
                        const errorMessages = error.response.data.errors;

                        // Формуємо одне повідомлення з усіх помилок
                        let fullErrorMessage = '';
                        for (const field in errorMessages) {
                            if (errorMessages.hasOwnProperty(field)) {
                                fullErrorMessage += `${field}: ${errorMessages[field].join(', ')}\n`;
                            }
                        }

                        // Виводимо повідомлення користувачу (наприклад, через alert або у ваш UI)
                        alert(fullErrorMessage);
                    }
                    onClose();
                });
        }
    };

    return (
        <BaseModal open={open} onClick={onClose}>
            <form onSubmit={handleSubmit}>
                <h5>{accident ? 'Редагування інциденту' : 'Створення інциденту'}</h5>
                <div className="mb-3">
                    <label htmlFor="Date" className="form-label">Date</label>
                    <input
                        type="date"
                        className="form-control"
                        id="Date"
                        name="Date"
                        value={formData.Date}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="Location" className="form-label">Location</label>
                    <input
                        type="text"
                        className="form-control"
                        id="Location"
                        name="Location"
                        value={formData.Location}
                        onChange={handleChange}
                        maxLength="100"
                        required
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="Description" className="form-label">Description</label>
                    <textarea
                        className="form-control"
                        id="Description"
                        name="Description"
                        value={formData.Description}
                        onChange={handleChange}
                        maxLength="255"
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
