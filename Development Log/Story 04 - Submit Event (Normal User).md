# Story 4: Normal user can submit an event for approval

As a normal user, I want to submit an event so an approver can review it before it goes on
the calendar.

| # | Task | Est. | Actual | Status |
|---|------|------|--------|--------|
| 1 | Create a "new event" request model | 0.5h | part of the Stories 4-11 backend batch (~23 min total — see Story 11) | Done |
| 2 | Write the `POST /api/events` endpoint | 1h | part of the same batch | Done |
| 3 | Make the endpoint save the event as pending for normal users | 0.5h | part of the same batch | Done |
| 4 | Add server-side validation for title, description, and location | 1h | — | Done |
| 5 | Build the React "submit event" form | 1.5h | part of a combined frontend CRUD batch (~24 min total — see Story 11) | Done |
| 6 | Add client-side validation matching the server rules | 1h | — | Not done — tracked as an open gap, not a blocker |
| 7 | Call the submit-event API from the form | 0.5h | part of the frontend CRUD batch | Done |
| 8 | Show a success message after submitting | 0.25h | — | Done |
| 9 | Show validation errors on the form | 0.5h | — | Partial — shows the backend's raw error message, not per-field validation |

**Story 4 total — Est: 6.75h**
