# Story 3: User can sign in

As a user, I want to sign in with my logon name and password so I can use the app.

| # | Task | Est. | Actual | Status |
|---|------|------|--------|--------|
| 1 | Install the BCrypt.Net-Next NuGet package | 0.25h | 0.03h | Done |
| 2 | Write a method to hash a password with BCrypt | 0.25h | 0.1h (combined with Task 3) | Done |
| 3 | Write a method to check a password against its hash | 0.25h | included with Task 2 | Done |
| 4 | Create a Login request model | 0.25h | 0.05h | Done |
| 5 | Write the `POST /api/auth/login` endpoint | 1h | 0.13h | Done |
| — | *(unplanned)* Add a working Swagger UI page | — | 0.08h | Done |
| — | *(unplanned)* Switch from SQL Server LocalDB to SQLite (environment issue) | — | 0.17h | Done |
| 6 | Install the JWT NuGet package and add JWT settings | 0.5h | 0.3h | Done |
| 7 | Write a method that builds a JWT with the user's id and approver claim | 1h | 0.15h | Done |
| 8 | Wire up JWT authentication in `Program.cs` | 0.75h | included with Task 7 | Done |
| 9 | Test the login endpoint in Swagger | 0.5h | 0.05h | Done |
| — | *(unplanned)* Install React Router and set up basic page routing | — | 0.25h | Done |
| 10 | Build the React sign-in page | 1.5h | 0.5h | Done |
| 11 | Create an `AuthContext` to hold the token and approver flag | 1h | 0.58h | Done |
| — | *(unplanned)* Add a CORS policy to the backend | — | 0.1h (included in Task 12) | Done |
| 12 | Call the login API from the sign-in page and save the token | 1h | 0.33h (combined with Tasks 13-14) | Done |
| 13 | Redirect to the calendar page after a successful sign-in | 0.25h | included above | Done |
| 14 | Show an error message when sign-in fails | 0.5h | included above | Done |

**Story 3 total — Est: 9h, Actual: ~2.7h** (plus the unplanned Swagger/SQLite work above, which
isn't reflected in that total)
