# Story 11: Approver can manage events directly

As an approver, I want to edit or delete any approved event so I can fix mistakes without
going through the approval queue.

| # | Task | Est. | Status |
|---|------|------|--------|
| 1 | Write `PUT /api/events/{id}` (approver-only) | 0.75h | Done |
| 2 | Write `DELETE /api/events/{id}` for already-approved events (approver-only) | 0.5h | Done |
| 3 | Enforce the "max 10 approved events per day" rule on create/approve/edit | 0.75h | Done |
| 4 | Build an Events management page listing all approved events | 1h | Done |
| 5 | Add Edit and Delete buttons to each event | 1h | Done, live-tested |
| 6 | Wire the edit form and delete button to the API | 1h | Done |

**Note:** the backend endpoints for Stories 4, 5, 6, 7 (Task 1), 8 (Task 1), 10, and 11 (Tasks
1-3) were all written together in a single pass rather than one story at a time, since the
full API surface was already known. The repetitive CRUD screens for Stories 4 (Task 5), 6
(Tasks 4-6), 10 (Tasks 5-8), and 11 (Tasks 4-6) were also built together in one pass, then
extensively live-tested against the real database afterward.

**Story 11 total — Est: 5h**
