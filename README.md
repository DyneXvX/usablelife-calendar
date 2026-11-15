# School Event Calendar

## Description

A browser-based school calendar application. Users can view a shared event calendar, submit
new events, and (for approvers) review, approve, edit, and delete events and user accounts.

**Project structure:**
- `UsableLifeCalendar.Api` — the backend API
- `UsableLifeCalendar.Api.Tests` — automated backend tests
- `usablelife-calendar-ui` — the frontend
- `UsableLifeCalendar.slnx` — solution file tying the two .NET projects together

**Built with:**
- Backend: ASP.NET Core Web API (.NET 10), Entity Framework Core, SQLite
- Frontend: React 18 with TypeScript, Vite
- Authentication: JWT, with passwords hashed using BCrypt

## Getting Started

### Dependencies

- .NET 10 SDK or later
- Node.js 20 or later, with npm
- Visual Studio 2022/2026 or another editor capable of opening a `.slnx` solution (optional —
  the backend can also be run entirely from the command line)

### Installing

1. Clone the repository:

```
git clone <repository-url>
cd usablelife-calendar
```

2. Set up the backend's signing key. The backend signs login tokens with a secret key. This key
   is intentionally **not** stored in the repository — it's kept in .NET User Secrets, which
   live outside the project folder on your own machine. From inside the `UsableLifeCalendar.Api`
   folder, run:

```
dotnet user-secrets init
```

   Then generate a random value to use as the key — for example, in PowerShell, from inside the
   `UsableLifeCalendar.Api` folder:

```
PS C:\git\usablelife-calendar\UsableLifeCalendar.Api> [System.Guid]::NewGuid().ToString()
103f81c3-9ae3-4ed8-810d-4130e232356a
```

   That output will be different every time you run it — the value above is only an example.
   Copy whatever your own command actually prints, then set it as the key:

```
dotnet user-secrets set "Jwt:Key" "103f81c3-9ae3-4ed8-810d-4130e232356a"
```

   Without this step, the application will still start, but logging in will fail.

3. Install the frontend's dependencies:

```
cd usablelife-calendar-ui
npm install
```

### Executing the Program

**Run the backend:** open `UsableLifeCalendar.slnx` and run the `UsableLifeCalendar.Api`
project (or run `dotnet run` from inside the `UsableLifeCalendar.Api` folder). The database is
created and seeded automatically on first run — no manual database setup is required.

> If Visual Studio doesn't default to running the API project, right-click
> `UsableLifeCalendar.Api` in Solution Explorer and choose **Set as Startup Project**.

The API will be available at `https://localhost:7181`, with Swagger documentation at
`https://localhost:7181/swagger`.

**Run the frontend** in a separate terminal:

```
cd usablelife-calendar-ui
npm run dev
```

Then open the URL shown in the terminal (typically `http://localhost:5173`).

Both the backend and frontend need to be running at the same time — they are two separate
processes.

**Test accounts** — the database is seeded with two accounts for trying out both permission
levels:

| Logon Name | Password     | Role          |
|------------|--------------|---------------|
| jsmith     | Password1!   | Normal user   |
| mjohnson   | Approver1!   | Approver      |

**Run the automated tests** from the repository root:

```
dotnet test UsableLifeCalendar.Api.Tests
```

## Development Log

The [`Development Log`](./Development%20Log/) folder breaks down every user story for this
project. Each one lists the tasks involved, the estimated hours, and the actual hours. It was
created by, and is monitored with, Claude Code.

## Authors

Name: Justin Thoms  
Email: Thoms.Justin@outlook.com

Feedback is always welcome — feel free to reach out.

## Acknowledgments

- [Claude Code](https://claude.com/claude-code) — used throughout the development of this
  project.
- [Microsoft Learn](https://learn.microsoft.com/) — ASP.NET Core and Entity Framework Core
  documentation.
- [React documentation](https://react.dev/) — frontend reference.
- [Define a default startup project in your .slnx file](https://www.poppastring.com/blog/define-a-default-startup-project-in-your-slnx-file)
  — used to set up the solution so Visual Studio defaults to running the right project.
- Calendar grid layout research (how other calendar UIs handle the leading/trailing days of a
  month): [CoreUI Scheduler](https://coreui.io/scheduler/docs/views/month/),
  [A Notion-style calendar is just one CSS grid](https://dev.to/dev48v/a-notion-style-calendar-is-just-one-css-grid-3ddi)
