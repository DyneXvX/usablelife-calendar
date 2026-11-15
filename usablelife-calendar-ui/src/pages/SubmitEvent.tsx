import { useState } from 'react';
import type { SubmitEvent as ReactSubmitEvent } from 'react';
import { useAuth } from '../context/AuthContext';
import { apiFetch } from '../api/api';
import { API_BASE_URL } from '../api/config';
import { parseErrorMessage } from '../utils/parseErrorMessage';

/**
 * Form for submitting a new event. Normal users get it saved as pending;
 * approvers get it approved automatically.
 */
function SubmitEvent() {
    const { token } = useAuth();

    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [location, setLocation] = useState('');
    const [eventDate, setEventDate] = useState('');
    const [eventTime, setEventTime] = useState('');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    async function handleSubmit(e: ReactSubmitEvent) {
        e.preventDefault();
        setError('');
        setSuccess('');

        const response = await apiFetch(`${API_BASE_URL}/api/events`, token, {
            method: 'POST',
            body: JSON.stringify({
                title,
                description,
                location,
                eventDate,
                eventTime: eventTime + ':00',
            }),
        });

        if (!response.ok) {
            const message = await parseErrorMessage(response);
            setError(message || 'Could not submit the event. Please check your entries.');
            return;
        }

        setSuccess('Event submitted successfully.');
        setTitle('');
        setDescription('');
        setLocation('');
        setEventDate('');
        setEventTime('');
    }

    return (
        <div className="page">
            <h1>Submit Event</h1>
            <form onSubmit={handleSubmit}>
                <div className="field">
                    <label>Title</label>
                    <input type="text" value={title} onChange={(e) => setTitle(e.target.value)} />
                </div>
                <div className="field">
                    <label>Description</label>
                    <textarea value={description} onChange={(e) => setDescription(e.target.value)} />
                </div>
                <div className="field">
                    <label>Location</label>
                    <input type="text" value={location} onChange={(e) => setLocation(e.target.value)} />
                </div>
                <div className="field">
                    <label>Date</label>
                    <input type="date" value={eventDate} onChange={(e) => setEventDate(e.target.value)} />
                </div>
                <div className="field">
                    <label>Time</label>
                    <input type="time" value={eventTime} onChange={(e) => setEventTime(e.target.value)} />
                </div>
                {error && <p className="error">{error}</p>}
                {success && <p className="success">{success}</p>}
                <button type="submit" className="btn btn-primary">Submit</button>
            </form>
        </div>
    );
}

export default SubmitEvent;
