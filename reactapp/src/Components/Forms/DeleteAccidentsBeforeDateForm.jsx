import { useState, useEffect } from 'react';
import BaseModal from './BaseModal';
import axios from 'axios'; // Import axios
import 'bootstrap/dist/css/bootstrap.min.css';

export default function DeleteAccidentsBeforeDateForm({ open, onClose }) { 

    const [date, setDate] = useState('');

    const handleChange = (e) => {
        const { name, value } = e.target;
        setDate((prev) => (value));
    };

    function showToast(message, duration = 3000) {
        const toastContainer = document.getElementById('toast-container');

        const toast = document.createElement('div');
        toast.className = 'toast';
        toast.textContent = message;

        toastContainer.appendChild(toast);

       
        setTimeout(() => {
            toast.classList.add('show');
        }, 10);

        // Видалити після закінчення часу
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => toast.remove(), 500);
        }, duration);
    }


    const handleSubmit = (e) => {
        e.preventDefault();
        if (date != '') {
            axios.post('/api/Accidents/DeleteAccidentsBeforeDate',{"date" : date})
                .then(response => {
                    console.log(response)
                    showToast(response.data.message);
                    onClose();
                })
                .catch(error => {

                    alert("Error while deleating");

                    onClose();
                    console.error('Помилка :', error);
                });
        };
    };

    return (
        <BaseModal open={open} onClick={onClose}>
            <form onSubmit={handleSubmit} style={{ width: 400 }}>
                <h5>{"Enter Date"}</h5>
                <div className="mb-3">
                    <label htmlFor="Date" className="form-label">Date</label>
                    <input
                        type="date"
                        className="form-control"
                        id="Date"
                        name="Date"
                        value={date}
                        onChange={handleChange}
                        required
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
