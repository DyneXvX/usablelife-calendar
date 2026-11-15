import { useState } from 'react';
import MonthView from './MonthView';
import DayView from './DayView';

function Calendar() {
    const today = new Date();
    const [view, setView] = useState<'month' | 'day'>('month');
    const [year, setYear] = useState(today.getFullYear());
    const [month, setMonth] = useState(today.getMonth() + 1);
    const [day, setDay] = useState(today.getDate());

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

    function goToPreviousDay() {
        const current = new Date(year, month - 1, day);
        current.setDate(current.getDate() - 1);
        setYear(current.getFullYear());
        setMonth(current.getMonth() + 1);
        setDay(current.getDate());
    }

    function goToNextDay() {
        const current = new Date(year, month - 1, day);
        current.setDate(current.getDate() + 1);
        setYear(current.getFullYear());
        setMonth(current.getMonth() + 1);
        setDay(current.getDate());
    }

    function handleDayClick(clickedDay: number) {
        setDay(clickedDay);
        setView('day');
    }

    function goToMonthView() {
        setView('month');
    }

    function goToToday() {
        const now = new Date();
        setYear(now.getFullYear());
        setMonth(now.getMonth() + 1);
        setDay(now.getDate());
    }

    if (view === 'month') {
        return (
            <MonthView
                year={year}
                month={month}
                onPreviousMonth={goToPreviousMonth}
                onNextMonth={goToNextMonth}
                onDayClick={handleDayClick}
                onToday={goToToday}
            />
        );
    }

    return (
        <DayView
            year={year}
            month={month}
            day={day}
            onPreviousDay={goToPreviousDay}
            onNextDay={goToNextDay}
            onMonthView={goToMonthView}
            onToday={goToToday}
        />
    );
}

export default Calendar;
