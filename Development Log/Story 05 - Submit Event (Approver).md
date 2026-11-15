# Story 5: Approver's submitted events go straight on the calendar

As an approver, I want my own submitted events to be approved automatically so I don't need
a second step for my own entries.

| # | Task | Est. | Actual | Status |
|---|------|------|--------|--------|
| 1 | Update `POST /api/events` to check if the logged-in user is an approver | 0.5h | part of the Stories 4-11 backend batch (~23 min total — see Story 11) | Done |
| 2 | If the user is an approver, set approved-by and approved-at automatically | 0.5h | part of the same batch | Done |
| 3 | Test this in Swagger with an approver account and a normal account | 0.5h | — | Skipped on purpose — covered instead by the automated tests in Story 12 and real frontend usage |

**Story 5 total — Est: 1.5h**
