/**
 * Formats an ISO date string from the API (like "2026-11-14T00:00:00") as a
 * plain date, like "11/14/2026".
 * @param isoDate The ISO date string from the API.
 * @returns The formatted date.
 */
export function formatDate(isoDate: string) {
    const date = new Date(isoDate);
    return date.toLocaleDateString('en-US');
}
