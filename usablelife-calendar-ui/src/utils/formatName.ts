/**
 * Formats a name as "Last, First Middle", matching the display format the spec requires.
 * @param firstName The first name.
 * @param middleName The middle name, or null/empty if there isn't one.
 * @param lastName The last name.
 * @returns The formatted name.
 */
export function formatName(firstName: string, middleName: string | null, lastName: string) {
    if (middleName) {
        return `${lastName}, ${firstName} ${middleName}`;
    }

    return `${lastName}, ${firstName}`;
}
