//common interface for all Events
export interface CalendarEvent {
    eventId: number;
    title: string;
    description: string;
    location: string | null;
    eventDate: string;
    eventTime: string;
    createdByFirstName: string;
    createdByMiddleName: string | null;
    createdByLastName: string;
}