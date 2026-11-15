# Story 10: Approver can manage users

As an approver, I want to create, view, edit, and delete user accounts so I can manage who
has access to the app.

| # | Task | Est. | Status |
|---|------|------|--------|
| 1 | Write `GET /api/users` (approver-only) | 0.5h | Done |
| 2 | Write `POST /api/users` (create a user, hash the password) | 0.75h | Done |
| 3 | Write `PUT /api/users/{id}` (edit a user) | 0.75h | Done |
| 4 | Write `DELETE /api/users/{id}` | 0.5h | Done |
| 5 | Build the React Users list page | 1h | Done |
| 6 | Build the Create/Edit User form with validation | 1.5h | Done (no client-side validation yet) |
| 7 | Add a Delete button with a confirm step | 0.5h | Done (uses the browser's native confirm dialog, flagged for the styling pass) |
| 8 | Wire the Users page to the API (list, create, edit, delete) | 1h | Done, live-tested |
| 9 | Format creator/approver names as "Last, First Middle" everywhere they show | 0.5h | Done |

**Story 10 total — Est: 7h**
