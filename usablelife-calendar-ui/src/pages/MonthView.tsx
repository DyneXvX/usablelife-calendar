import { useState, useEffect } from "react";
import { useAuth } from '../context/AuthContext';
import { apiFetch } from '../api/api';
import { API_BASE_URL } from '../api/config';
import type { CalendarEvent } from "../interfaces/Event";
import { formatTime } from '../utils/formatTime';

interface MonthViewProps {
    year: number;
    month: number;
    onPreviousMonth: () => void;
    onNextMonth: () => void;
    onDayClick: (day: number) => void;
}

function MonthView({ year, month, onPreviousMonth, onNextMonth, onDayClick }: MonthViewProps) {
    const { token } = useAuth();
    const [events, setEvents] = useState<CalendarEvent[]>([]);
    const [error, setError] = useState('')

    //back to 0 index as I added 1 to month above and it messes up this logic so remove the 1.
    const firstDayOfWeek = new Date(year, month - 1, 1).getDay();
    const daysInMonth = new Date(year, month, 0).getDate();

    const monthName = new Date(year, month - 1, 1).toLocaleString('en-US', { month: 'long' });

    //if the first day of the month is Wednesday, this will add [null, null, null, 1, 2] ((Sunday, Monday, Tuesday) <-- Last Month)
    const calendarCells = [];
    for (let i = 0; i < firstDayOfWeek; i++) {
        calendarCells.push(null);
    }

    for (let day = 1; day <= daysInMonth; day++) {
        calendarCells.push(day);
    }

    while (calendarCells.length % 7 !== 0) {
        calendarCells.push(null);
    }

    const weekdayNames = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];

    function getEventsForDay(day: number) {
        const matching: CalendarEvent[] = [];
        for (let i = 0; i < events.length; i++) {
            const eventDay = new Date(events[i].eventDate).getDate();
            if (eventDay === day) {
                matching.push(events[i]);
            }
        }
        //required in ascending time order.
        matching.sort((a, b) => (a.eventTime < b.eventTime ? -1 : 1));
        return matching;
    }

    useEffect(() => { LoadCalendar(); }, [year, month]);

    async function LoadCalendar() {
        const response = await apiFetch(`${API_BASE_URL}/api/events?year=${year}&month=${month}`, token)

        if (!response.ok) {
            setError('Could not load events for this month.');
            return;
        }

        const data = await response.json();
        setEvents(data);
    }

    return (
        <div>
            <h1>Month View</h1>
            <h2>{monthName} {year}</h2>
            <button type="button" onClick={onPreviousMonth}>Previous</button>
            <button type="button" onClick={onNextMonth}>Next</button>
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(7, 1fr)', gap: '4px' }}>
                {weekdayNames.map((name) => (
                    <div key={name} style={{ fontWeight: 'bold', textAlign: 'center' }}>
                        {name}
                    </div>
                ))}
                {calendarCells.map((day, index) => (
                    <div
                        key={index}
                        onClick={() => day && onDayClick(day)}
                        style={{
                            border: '1px solid #ccc',
                            minHeight: '80px',
                            padding: '4px',
                            cursor: day ? 'pointer' : 'default'
                        }}
                    >
                        {day && (
                            <>
                                <strong>{day}</strong>
                                {getEventsForDay(day).map((e) => (
                                    <div key={e.eventId}>
                                        {formatTime(e.eventTime)} - {e.title} - {e.location}
                                    </div>
                                ))}
                            </>
                        )}
                    </div>
                ))}
            </div>
            {error && <p>{error}</p>}
        </div>
    );
}

export default MonthView;
