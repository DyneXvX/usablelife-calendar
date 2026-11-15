import { useState } from 'react';
import type { SubmitEvent as ReactSubmitEvent } from 'react';
import { useAuth } from '../context/AuthContext';
import { apiFetch } from '../api/api';

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

        const response = await apiFetch('https://localhost:7181/api/events', token, {
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
            const message = await response.text();
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
        <div>
            <h1>Submit Event</h1>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Title</label>
                    <input type="text" value={title} onChange={(e) => setTitle(e.target.value)} />
                </div>
                <div>
                    <label>Description</label>
                    <textarea value={description} onChange={(e) => setDescription(e.target.value)} />
                </div>
                <div>
                    <label>Location</label>
                    <input type="text" value={location} onChange={(e) => setLocation(e.target.value)} />
                </div>
                <div>
                    <label>Date</label>
                    <input type="date" value={eventDate} onChange={(e) => setEventDate(e.target.value)} />
                </div>
                <div>
                    <label>Time</label>
                    <input type="time" value={eventTime} onChange={(e) => setEventTime(e.target.value)} />
                </div>
                {error && <p>{error}</p>}
                {success && <p>{success}</p>}
                <button type="submit">Submit</button>
            </form>
        </div>
    );
}

export default SubmitEvent;
