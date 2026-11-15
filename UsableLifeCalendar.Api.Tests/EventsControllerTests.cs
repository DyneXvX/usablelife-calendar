using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UsableLifeCalendar.Api.Controllers;
using UsableLifeCalendar.Api.Data;
using UsableLifeCalendar.Api.DTOs;
using UsableLifeCalendar.Api.Models;

namespace UsableLifeCalendar.Api.Tests
{
    public class EventsControllerTests
    {
        private static ApplicationDbContext CreateContext()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        private static IConfiguration CreateConfig(int maxApprovedPerDay = 10)
        {
            Dictionary<string, string?> settings = new()
            {
                ["EventRules:MaxApprovedPerDay"] = maxApprovedPerDay.ToString()
            };

            return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        }

        private static EventsController CreateController(ApplicationDbContext context, IConfiguration config, int userId, bool isApprover)
        {
            EventsController controller = new(context, config);

            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("IsApprover", isApprover.ToString())
            ];

            ClaimsIdentity identity = new(claims, "TestAuth");
            ClaimsPrincipal user = new(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return controller;
        }

        [Fact]
        public void CreateEvent_AsNormalUser_IsSavedAsPending()
        {
            // Arrange
            ApplicationDbContext context = CreateContext();
            EventsController controller = CreateController(context, CreateConfig(), userId: 1, isApprover: false);

            CreateEventRequest request = new()
            {
                Title = "Fall Festival",
                Description = "Games and food.",
                Location = "Main Gym",
                EventDate = DateTime.Today,
                EventTime = new TimeOnly(14, 0)
            };

            // Act
            controller.CreateEvent(request);

            // Assert
            Event savedEvent = context.Events.Single();
            Assert.Null(savedEvent.ApprovedAt);
            Assert.Null(savedEvent.ApproverUserId);
        }

        [Fact]
        public void CreateEvent_AsApprover_IsApprovedImmediately()
        {
            // Arrange
            ApplicationDbContext context = CreateContext();
            EventsController controller = CreateController(context, CreateConfig(), userId: 2, isApprover: true);

            CreateEventRequest request = new()
            {
                Title = "Fall Festival",
                Description = "Games and food.",
                Location = "Main Gym",
                EventDate = DateTime.Today,
                EventTime = new TimeOnly(14, 0)
            };

            // Act
            controller.CreateEvent(request);

            // Assert
            Event savedEvent = context.Events.Single();
            Assert.NotNull(savedEvent.ApprovedAt);
            Assert.Equal(2, savedEvent.ApproverUserId);
        }

        [Fact]
        public void CreateEvent_AsApprover_WhenDayIsFull_ReturnsBadRequestAndDoesNotSave()
        {
            // Arrange
            ApplicationDbContext context = CreateContext();
            DateTime eventDate = DateTime.Today;

            for (int i = 0; i < 10; i++)
            {
                context.Events.Add(new Event
                {
                    Title = "Existing Event",
                    Description = "Already approved.",
                    EventDate = eventDate,
                    EventTime = new TimeOnly(9, 0),
                    CreatedByUserId = 2,
                    ApproverUserId = 2,
                    ApprovedAt = DateTime.UtcNow
                });
            }
            context.SaveChanges();

            EventsController controller = CreateController(context, CreateConfig(), userId: 2, isApprover: true);

            CreateEventRequest request = new()
            {
                Title = "One Too Many",
                Description = "Should not be allowed.",
                EventDate = eventDate,
                EventTime = new TimeOnly(15, 0)
            };

            // Act
            IActionResult result = controller.CreateEvent(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(10, context.Events.Count());
        }

        [Fact]
        public void ApproveEvent_AsApprover_SetsApprovedAtAndApproverUserId()
        {
            // Arrange
            ApplicationDbContext context = CreateContext();

            Event pendingEvent = new()
            {
                Title = "Pending Event",
                Description = "Waiting for approval.",
                EventDate = DateTime.Today,
                EventTime = new TimeOnly(10, 0),
                CreatedByUserId = 1
            };
            context.Events.Add(pendingEvent);
            context.SaveChanges();

            EventsController controller = CreateController(context, CreateConfig(), userId: 2, isApprover: true);

            // Act
            controller.ApproveEvent(pendingEvent.EventId);

            // Assert
            Event approvedEvent = context.Events.Single();
            Assert.NotNull(approvedEvent.ApprovedAt);
            Assert.Equal(2, approvedEvent.ApproverUserId);
        }

        [Fact]
        public void ApproveEvent_AsNormalUser_ReturnsForbidAndDoesNotApprove()
        {
            // Arrange
            ApplicationDbContext context = CreateContext();

            Event pendingEvent = new()
            {
                Title = "Pending Event",
                Description = "Waiting for approval.",
                EventDate = DateTime.Today,
                EventTime = new TimeOnly(10, 0),
                CreatedByUserId = 1
            };
            context.Events.Add(pendingEvent);
            context.SaveChanges();

            EventsController controller = CreateController(context, CreateConfig(), userId: 1, isApprover: false);

            // Act
            IActionResult result = controller.ApproveEvent(pendingEvent.EventId);

            // Assert
            Assert.IsType<ForbidResult>(result);
            Assert.Null(context.Events.Single().ApprovedAt);
        }

        [Fact]
        public void ApproveEvent_WhenDayIsFull_ReturnsBadRequestAndDoesNotApprove()
        {
            // Arrange
            ApplicationDbContext context = CreateContext();
            DateTime eventDate = DateTime.Today;

            for (int i = 0; i < 10; i++)
            {
                context.Events.Add(new Event
                {
                    Title = "Existing Event",
                    Description = "Already approved.",
                    EventDate = eventDate,
                    EventTime = new TimeOnly(9, 0),
                    CreatedByUserId = 2,
                    ApproverUserId = 2,
                    ApprovedAt = DateTime.UtcNow
                });
            }

            Event pendingEvent = new()
            {
                Title = "Pending Event",
                Description = "Waiting for approval.",
                EventDate = eventDate,
                EventTime = new TimeOnly(16, 0),
                CreatedByUserId = 1
            };
            context.Events.Add(pendingEvent);
            context.SaveChanges();

            EventsController controller = CreateController(context, CreateConfig(), userId: 2, isApprover: true);

            // Act
            IActionResult result = controller.ApproveEvent(pendingEvent.EventId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Null(context.Events.Single(e => e.EventId == pendingEvent.EventId).ApprovedAt);
        }

        [Fact]
        public void DeleteEvent_AsApprover_RemovesEventFromDatabase()
        {
            // Arrange
            ApplicationDbContext context = CreateContext();

            Event eventToDelete = new()
            {
                Title = "Unwanted Event",
                Description = "Should be removed.",
                EventDate = DateTime.Today,
                EventTime = new TimeOnly(11, 0),
                CreatedByUserId = 1
            };
            context.Events.Add(eventToDelete);
            context.SaveChanges();

            EventsController controller = CreateController(context, CreateConfig(), userId: 2, isApprover: true);

            // Act
            controller.DeleteEvent(eventToDelete.EventId);

            // Assert
            Assert.Empty(context.Events);
        }

        [Fact]
        public void DeleteEvent_AsNormalUser_ReturnsForbidAndDoesNotDelete()
        {
            // Arrange
            ApplicationDbContext context = CreateContext();

            Event existingEvent = new()
            {
                Title = "Existing Event",
                Description = "Should not be deletable by a normal user.",
                EventDate = DateTime.Today,
                EventTime = new TimeOnly(11, 0),
                CreatedByUserId = 1
            };
            context.Events.Add(existingEvent);
            context.SaveChanges();

            EventsController controller = CreateController(context, CreateConfig(), userId: 1, isApprover: false);

            // Act
            IActionResult result = controller.DeleteEvent(existingEvent.EventId);

            // Assert
            Assert.IsType<ForbidResult>(result);
            Assert.NotEmpty(context.Events);
        }
    }
}
