//customer interface for all Events
export interface CalendarEvent {
    eventId: number;
    title: string;
    location: string | null;
    eventDate: string;
    eventTime: string;
}