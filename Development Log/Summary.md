# Development Log — Summary

This folder is a factual record of how this project was built: each file covers one user
story from the original plan, listing the story itself, the tasks under it, and how long each
task was estimated to take.

**Format:** each story is written as "As a [user], I want [thing], so that [reason]" — the
story explains the goal and does not get its own hour estimate. Under each story is an
ordered list of tasks, each with an **Est.** (estimated before starting).

A few tasks across Stories 4-11 (backend CRUD endpoints for events and users) were written
together in a single batch rather than one at a time, since the full API surface was already
known.

## Totals

| Story | Est. Hours |
|---|---|
| 1 — Project setup | 3h |
| 2 — Database tables | 4h |
| 3 — Sign in | 9h |
| 4 — Submit event (normal user) | 6.75h |
| 5 — Submit event (approver, auto-approved) | 1.5h |
| 6 — Review pending events | 6h |
| 7 — Month view | 6.25h |
| 8 — Day view | 4h |
| 9 — Toggle month/day | 1.5h |
| 10 — Manage users | 7h |
| 11 — Manage events directly | 5h |
| 12 — Automated tests | 3.5h |
| 13 — Ready to submit | 4.5h + a styling pass (not separately estimated) |
| **TOTAL** | **~62.25h** |

**Optional spec items (drag/drop, Locations CRUD, event images) are not included above** —
they were out of scope unless time remained after Story 13.
