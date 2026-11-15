/**
 * Reads an error response from the API and returns a readable message.
 * Handles both kinds of error the backend can send - the automatic one when
 * a field is invalid, and the plain-text ones we write ourselves (like the
 * 10-per-day limit message).
 * @param response The failed API response.
 * @returns A readable error message.
 */
export async function parseErrorMessage(response: Response) {
    const text = await response.text();

    try {
        const parsed = JSON.parse(text);

        if (parsed.errors) {
            const messages: string[] = [];
            for (const field in parsed.errors) {
                for (const message of parsed.errors[field]) {
                    messages.push(message);
                }
            }
            return messages.join(' ');
        }
    } catch {
        // Not JSON, fall through and use the plain text below.
    }

    return text;
}
