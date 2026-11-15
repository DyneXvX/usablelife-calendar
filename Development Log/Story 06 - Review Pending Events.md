# Story 6: Approver can review pending events

As an approver, I want to see events waiting for approval so I can approve or delete them.

| # | Task | Est. | Actual | Status |
|---|------|------|--------|--------|
| 1 | Write the `GET /api/events/pending` endpoint (approver-only) | 0.75h | part of the Stories 4-11 backend batch (~23 min total — see Story 11) | Done |
| 2 | Write the `POST /api/events/{id}/approve` endpoint | 0.75h | part of the same batch | Done |
| 3 | Write the `DELETE /api/events/{id}` endpoint for pending events | 0.5h | part of the same batch | Done |
| 4 | Build the React "Pending Events" page, approver-only | 1.5h | part of a combined frontend CRUD batch (~24 min total — see Story 11) | Done |
| 5 | Add Approve and Delete buttons to each row | 1h | — | Done, live-tested |
| 6 | Call the approve/delete APIs from the buttons and refresh the list | 1h | — | Done |
| 7 | Block normal users from seeing this page | 0.5h | — | Done, live-tested both ways (hidden nav link and direct URL blocked) |

**Story 6 total — Est: 6h**
