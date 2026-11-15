# Story 12: Backend has automated tests

As a developer, I want a few automated tests so I know the important rules actually work.

| # | Task | Est. | Actual | Status |
|---|------|------|--------|--------|
| 1 | Create an xUnit test project | 0.25h | 0.15h | Done |
| 2 | Write tests for the logon name / password / name validation rules | 1h | 0.2h | Done — 10 tests |
| 3 | Write tests for the one-step vs. two-step submission logic | 1h | 0.15h | Done — 3 tests |
| 4 | Write tests for the approve/delete pending event logic | 0.75h | 0.2h | Done — 5 tests, including the first real test of the 10-per-day cap |
| 5 | Run all tests and fix anything that fails | 0.5h | 0.05h | Done — all 18 passed on the first run |

**Story 12 total — Est: 3.5h, Actual: ~0.75h**

`GetEvents`/`GetPendingEvents` (the "read" endpoints) are not tested — never in scope for this
story, only to be added later if there's real spare time.
