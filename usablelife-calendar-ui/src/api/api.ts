/**
 * Sends an API request, attaching the login token if there is one.
 * @param url The full URL to call.
 * @param token The login token to attach, or null if not logged in.
 * @param options Extra fetch options (method, body, etc).
 * @returns The raw fetch Response.
 */
export async function apiFetch(url: string, token: string | null, options: RequestInit = {}) {
    const headers = {
        ...options.headers,
        'Content-Type': 'application/json',
        Authorization: token ? `Bearer ${token}` : '',
    };

    return fetch(url, { ...options, headers });
}
