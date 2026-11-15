import { useEffect, useState } from 'react';
import type { SubmitEvent as ReactSubmitEvent } from 'react';
import { useAuth } from '../context/AuthContext';
import { apiFetch } from '../api/api';
import { API_BASE_URL } from '../api/config';
import { formatDate } from '../utils/formatDate';
import { formatTime } from '../utils/formatTime';

interface EventRecord {
    eventId: number;
    title: string;
    description: string;
    location: string | null;
    eventDate: string;
    eventTime: string;
}

/**
 * Approver-only page for managing every already-approved event: edit or delete any of them directly.
 */
function ManageEvents() {
    const { token } = useAuth();
    const [events, setEvents] = useState<EventRecord[]>([]);
    const [error, setError] = useState('');

    const [editingId, setEditingId] = useState<number | null>(null);
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [location, setLocation] = useState('');
    const [eventDate, setEventDate] = useState('');
    const [eventTime, setEventTime] = useState('');

    async function loadEvents() {
        const response = await apiFetch(`${API_BASE_URL}/api/events`, token);

        if (!response.ok) {
            setError('Could not load events.');
            return;
        }

        const data = await response.json();
        setEvents(data);
    }

    useEffect(() => {
        loadEvents();
    }, []);

    function startEdit(e: EventRecord) {
        setEditingId(e.eventId);
        setTitle(e.title);
        setDescription(e.description);
        setLocation(e.location ?? '');
        setEventDate(e.eventDate.substring(0, 10));
        setEventTime(e.eventTime.substring(0, 5));
    }

    async function handleSubmit(e: ReactSubmitEvent) {
        e.preventDefault();
        setError('');

        if (editingId === null) {
            return;
        }

        const response = await apiFetch(`${API_BASE_URL}/api/events/${editingId}`, token, {
            method: 'PUT',
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
            setError(message || 'Could not update the event.');
            return;
        }

        setEditingId(null);
        loadEvents();
    }

    async function handleDelete(id: number) {
        const confirmed = window.confirm('Delete this event?');

        if (!confirmed) {
            return;
        }

        await apiFetch(`${API_BASE_URL}/api/events/${id}`, token, {
            method: 'DELETE',
        });
        loadEvents();
    }

    return (
        <div>
            <h1>Manage Events</h1>
            {error && <p>{error}</p>}
            <table>
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Date</th>
                        <th>Time</th>
                        <th>Location</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {events.map((ev) => (
                        <tr key={ev.eventId}>
                            <td>{ev.title}</td>
                            <td>{formatDate(ev.eventDate)}</td>
                            <td>{formatTime(ev.eventTime)}</td>
                            <td>{ev.location}</td>
                            <td>
                                <button type="button" onClick={() => startEdit(ev)}>Edit</button>
                                <button type="button" onClick={() => handleDelete(ev.eventId)}>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {editingId !== null && (
                <form onSubmit={handleSubmit}>
                    <h2>Edit Event</h2>
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
                    <button type="submit">Save</button>
                    <button type="button" onClick={() => setEditingId(null)}>Cancel</button>
                </form>
            )}
        </div>
    );
}

export default ManageEvents;
