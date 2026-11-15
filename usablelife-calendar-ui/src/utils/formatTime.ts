/**
 * Formats a time string from the API (like "14:00:00") as a 12-hour time,
 * like "2:00 PM".
 * @param time The time string from the API (HH:mm:ss).
 * @returns The formatted time.
 */
export function formatTime(time: string) {
    const [hoursText, minutesText] = time.split(':');
    const hours = parseInt(hoursText, 10);
    const minutes = parseInt(minutesText, 10);

    const period = hours >= 12 ? 'PM' : 'AM';
    const displayHours = hours % 12 === 0 ? 12 : hours % 12;
    const displayMinutes = minutes.toString().padStart(2, '0');

    return `${displayHours}:${displayMinutes} ${period}`;
}
