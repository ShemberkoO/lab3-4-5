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
                  
                    onClose();
                })
                .catch(error => {
                   
                    if (error.response && error.response.status === 400) {
                        // �������� ������ ������� � ������ �������
                        const errorMessages = error.response.data.errors;

                        // ������� ���� ����������� � ��� �������
                        let fullErrorMessage = '';
                        for (const field in errorMessages) {
                            if (errorMessages.hasOwnProperty(field)) {
                                fullErrorMessage += `${field}: ${errorMessages[field].join(', ')}\n`;
                            }
                        }

                        // �������� ����������� ����������� (���������, ����� alert ��� � ��� UI)
                        alert(fullErrorMessage);
                    }
                    if (error.response && error.response.status === 401) {
                        alert(error.response.data.message + error.response.data.details);
                    }
                    onClose();
                });
        } else {
            axios.post('/api/Accidents', formData)
                .then(response => {
                    console.log('����� �������� ������ ��������:', response.data);
                    onClose();
                })
                .catch(error => {
                    console.log(error);
                    if (error.response && error.response.status === 400) {
                        // �������� ������ ������� � ������ �������
                        const errorMessages = error.response.data.errors;

                        // ������� ���� ����������� � ��� �������
                        let fullErrorMessage = '';
                        for (const field in errorMessages) {
                            if (errorMessages.hasOwnProperty(field)) {
                                fullErrorMessage += `${field}: ${errorMessages[field].join(', ')}\n`;
                            }
                        }

                        // �������� ����������� ����������� (���������, ����� alert ��� � ��� UI)
                        alert(error.response.data.message);
                    }
                    if (error.response && error.response.status === 401) {
                        alert(error.response.data.message + error.response.data.details);
                    }
                    onClose();
                });

        }
    };

    return (
        <BaseModal open={open} onClick={onClose}>
            <form onSubmit={handleSubmit} style={{ width: 400 }}>
                <h5>{accident ? '      Edit accident form      ' : '       Create accident form      '}</h5>
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
                    <button type="button" className="btn btn-secondary" onClick={onClose}>Close</button>
                    <button type="submit" className="btn btn-primary">Save</button>
                </div>
            </form>
        </BaseModal>
    );
}
