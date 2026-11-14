using UsableLifeCalendar.Api.Models;

namespace UsableLifeCalendar.Api.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // If there are already users in the database, assume it's already been
            // seeded, so we don't add duplicate data every time the app starts.
            if (context.Users.Any())
            {
                return;
            }

            // Passwords (for testing login later):
            // normal user "jsmith" -> Password1!    approver "mjohnson" -> Approver1!
            var normalUser = new User
            {
                FirstName = "Jane",
                LastName = "Smith",
                LogonName = "jsmith",
                PasswordHash = "$2a$11$Hrcdkp4W/gYt5kC6My007eMSuFby5eGDLXU3N/z6gEQrA.XC6xoe.",
                IsApprover = false
            };

            var approverUser = new User
            {
                FirstName = "Mike",
                LastName = "Johnson",
                LogonName = "mjohnson",
                PasswordHash = "$2a$11$aX0vm0D9oDnR8l/81/2tsOhY/GUAxWHWt3lm8jqw8RSYVSBeT/qD2",
                IsApprover = true
            };

            context.Users.AddRange(normalUser, approverUser);
            context.SaveChanges();

            // Saving the users above fills in their real UserId values, so now we can
            // use normalUser.UserId / approverUser.UserId below instead of guessing numbers.
            var events = new List<Event>
            {
                new Event
                {
                    Title = "Fall Festival",
                    Description = "Annual fall festival with games and food.",
                    Location = "Main Gym",
                    EventDate = new DateTime(2026, 11, 20),
                    EventTime = new TimeOnly(14, 0),
                    CreatedByUserId = approverUser.UserId,
                    ApproverUserId = approverUser.UserId,
                    CreatedAt = new DateTime(2026, 11, 1, 9, 0, 0),
                    ApprovedAt = new DateTime(2026, 11, 1, 9, 0, 0)
                },
                new Event
                {
                    Title = "Parent Night",
                    Description = "Meet the teachers night for all grades.",
                    Location = "Cafeteria",
                    EventDate = new DateTime(2026, 11, 20),
                    EventTime = new TimeOnly(18, 0),
                    CreatedByUserId = approverUser.UserId,
                    ApproverUserId = approverUser.UserId,
                    CreatedAt = new DateTime(2026, 11, 2, 10, 0, 0),
                    ApprovedAt = new DateTime(2026, 11, 2, 10, 0, 0)
                },
                new Event
                {
                    Title = "Winter Concert",
                    Description = "Holiday concert performed by the school band and choir.",
                    Location = "Auditorium",
                    EventDate = new DateTime(2026, 12, 5),
                    EventTime = new TimeOnly(19, 0),
                    CreatedByUserId = approverUser.UserId,
                    ApproverUserId = approverUser.UserId,
                    CreatedAt = new DateTime(2026, 11, 3, 9, 0, 0),
                    ApprovedAt = new DateTime(2026, 11, 3, 9, 0, 0)
                },
                new Event
                {
                    Title = "Bake Sale",
                    Description = "PTA bake sale to raise funds for the library.",
                    Location = "Front Lobby",
                    EventDate = new DateTime(2026, 11, 21),
                    EventTime = new TimeOnly(8, 0),
                    CreatedByUserId = normalUser.UserId,
                    ApproverUserId = null,
                    CreatedAt = new DateTime(2026, 11, 10, 12, 0, 0),
                    ApprovedAt = null
                },
                new Event
                {
                    Title = "Science Fair",
                    Description = "Student science fair projects on display.",
                    Location = "Gym",
                    EventDate = new DateTime(2026, 12, 10),
                    EventTime = new TimeOnly(10, 0),
                    CreatedByUserId = normalUser.UserId,
                    ApproverUserId = null,
                    CreatedAt = new DateTime(2026, 11, 11, 8, 0, 0),
                    ApprovedAt = null
                }
            };

            context.Events.AddRange(events);
            context.SaveChanges();
        }
    }
}
