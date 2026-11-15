import { useState, useEffect } from "react";
import { useAuth } from '../context/AuthContext';
import { apiFetch } from '../api/api';
import type { CalendarEvent } from "../interfaces/Event";
import { formatTime } from '../utils/formatTime';

function MonthView() {
    const today = new Date();
    const { token } = useAuth();
    const [events, setEvents] = useState<CalendarEvent[]>([]);
    const [error, setError] = useState('')


    //found out this stores as 0 index: Add 1 as November comes back 10 not 11. 
    const [month, setMonth] = useState(today.getMonth() + 1)
    const [year, setYear] = useState(today.getFullYear())

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

    //monthly logic has to cover 1 year to the next. 
    function goToPreviousMonth() {
        if (month === 1) {
            setMonth(12);
            setYear(year - 1);
        } else {
            setMonth(month - 1);
        }
    }

    function goToNextMonth() {
        if (month === 12) {
            setMonth(1);
            setYear(year + 1);
        } else {
            setMonth(month + 1);
        }
    }


    useEffect(() => { LoadCalendar(); }, [year, month]);

    async function LoadCalendar() {
        const response = await apiFetch(`https://localhost:7181/api/Events?year=${year}&month=${month}`, token)

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
            <button type="button" onClick={goToPreviousMonth}>Previous</button>
            <button type="button" onClick={goToNextMonth}>Next</button>
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(7, 1fr)', gap: '4px' }}>
                {calendarCells.map((day, index) => (
                    <div key={index} style={{ border: '1px solid #ccc', minHeight: '80px', padding: '4px' }}>
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
