import { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { apiFetch } from '../api/api';
import { formatDate } from '../utils/formatDate';
import { formatTime } from '../utils/formatTime';
import type { CalendarEvent } from '../interfaces/Event';

/**
 * Approver-only page listing events waiting for approval, with Approve and
 * Delete buttons for each one.
 */
function PendingEvents() {
    const { token } = useAuth();
    const [events, setEvents] = useState<CalendarEvent[]>([]);
    const [error, setError] = useState('');

    async function loadPendingEvents() {
        const response = await apiFetch('https://localhost:7181/api/events/pending', token);

        if (!response.ok) {
            setError('Could not load pending events.');
            return;
        }

        const data = await response.json();
        setEvents(data);
    }

    useEffect(() => {
        loadPendingEvents();
    }, []);

    async function handleApprove(id: number) {
        await apiFetch(`https://localhost:7181/api/events/${id}/approve`, token, {
            method: 'POST',
        });
        loadPendingEvents();
    }

    async function handleDelete(id: number) {
        await apiFetch(`https://localhost:7181/api/events/${id}`, token, {
            method: 'DELETE',
        });
        loadPendingEvents();
    }

    return (
        <div>
            <h1>Pending Events</h1>
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
                    {events.map((e) => (
                        <tr key={e.eventId}>
                            <td>{e.title}</td>
                            <td>{formatDate(e.eventDate)}</td>
                            <td>{formatTime(e.eventTime)}</td>
                            <td>{e.location}</td>
                            <td>
                                <button type="button" onClick={() => handleApprove(e.eventId)}>Approve</button>
                                <button type="button" onClick={() => handleDelete(e.eventId)}>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default PendingEvents;
