import { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import { apiFetch } from '../api/api';
import { API_BASE_URL } from '../api/config';
import type { CalendarEvent } from '../interfaces/Event';
import { formatTime } from '../utils/formatTime';
import { formatName } from '../utils/formatName';

interface DayViewProps {
    year: number;
    month: number;
    day: number;
    onPreviousDay: () => void;
    onNextDay: () => void;
    onMonthView: () => void;
    onToday: () => void;
}

function DayView({ year, month, day, onPreviousDay, onNextDay, onMonthView, onToday }: DayViewProps) {
    const { token } = useAuth();
    const [events, setEvents] = useState<CalendarEvent[]>([]);
    const [error, setError] = useState('');

    const date = new Date(year, month - 1, day);

    function dateParam() {
        return `${year}-${month}-${day}`;
    }

    async function LoadDay() {
        const response = await apiFetch(`${API_BASE_URL}/api/events?date=${dateParam()}`, token);

        if (!response.ok) {
            setError('Could not load events for this day.');
            return;
        }

        const data = await response.json();
        setEvents(data);
    }

    useEffect(() => { LoadDay(); }, [year, month, day]);

    return (
        <div>
            <h1>Day View</h1>
            <h2>{date.toLocaleDateString('en-US')}</h2>
            <button type="button" onClick={onPreviousDay}>Previous</button>
            <button type="button" onClick={onToday}>Today</button>
            <button type="button" onClick={onMonthView}>Month View</button>
            <button type="button" onClick={onNextDay}>Next</button>

            {events.map((e) => (
                <div key={e.eventId} style={{ border: '1px solid #ccc', margin: '8px 0', padding: '8px' }}>
                    <p>{formatTime(e.eventTime)} - {e.title}</p>
                    <p>Location: {e.location}</p>
                    <p>{e.description}</p>
                    <p>Created by: {formatName(e.createdByFirstName, e.createdByMiddleName, e.createdByLastName)}</p>
                </div>
            ))}

            {error && <p>{error}</p>}
        </div>
    );
}

export default DayView;
