using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsableLifeCalendar.Api.Data;
using UsableLifeCalendar.Api.DTOs;
using UsableLifeCalendar.Api.Models;

namespace UsableLifeCalendar.Api.Controllers
{
    /// <summary>
    /// Handles viewing, submitting, approving, and managing calendar events.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        /// <summary>
        /// Creates the controller with the database and app settings it needs.
        /// </summary>
        /// <param name="context">The database connection.</param>
        /// <param name="config">The app settings (used to read the max-events-per-day rule).</param>
        public EventsController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Reads the logged-in user's id out of their login token.
        /// </summary>
        /// <returns>The logged-in user's id.</returns>
        private int CurrentUserId()
        {
            string idText = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return int.Parse(idText);
        }

        /// <summary>
        /// Checks whether the logged-in user is an approver.
        /// </summary>
        /// <returns>True if the logged-in user is an approver, false if not.</returns>
        private bool IsApprover()
        {
            string isApproverText = User.FindFirst("IsApprover")!.Value;
            return isApproverText == "True";
        }

        /// <summary>
        /// Builds the version of an event sent back to the client, with the creator's name
        /// looked up instead of just their id.
        /// </summary>
        /// <param name="e">The event from the database.</param>
        /// <returns>The event details, including the creator's name.</returns>
        private EventResponse ToEventResponse(Event e)
        {
            User? creator = _context.Users.FirstOrDefault(u => u.UserId == e.CreatedByUserId);

            EventResponse response = new()
            {
                EventId = e.EventId,
                Title = e.Title,
                Description = e.Description,
                Location = e.Location,
                EventDate = e.EventDate,
                EventTime = e.EventTime,
                CreatedByUserId = e.CreatedByUserId,
                CreatedByFirstName = creator?.FirstName ?? string.Empty,
                CreatedByMiddleName = creator?.MiddleName,
                CreatedByLastName = creator?.LastName ?? string.Empty,
                ApproverUserId = e.ApproverUserId,
                CreatedAt = e.CreatedAt,
                ApprovedAt = e.ApprovedAt
            };

            return response;
        }

        /// <summary>
        /// Reads the maximum number of approved events allowed on a single day from the app settings.
        /// </summary>
        /// <returns>The configured maximum.</returns>
        private int MaxApprovedPerDay()
        {
            return int.Parse(_config["EventRules:MaxApprovedPerDay"]!);
        }

        /// <summary>
        /// Counts how many approved events exist on a given day.
        /// </summary>
        /// <param name="date">The day to check.</param>
        /// <returns>The number of approved events on that day.</returns>
        private int ApprovedCountForDay(DateTime date)
        {
            int count = 0;

            foreach (Event e in _context.Events)
            {
                if (e.ApprovedAt != null && e.EventDate.Date == date.Date)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Gets approved events, for the month view, the day view, or the Manage Events page.
        /// </summary>
        /// <param name="year">The year to filter by, for the month view.</param>
        /// <param name="month">The month to filter by, for the month view.</param>
        /// <param name="date">The exact date to filter by, for the day view.</param>
        /// <returns>The matching approved events, sorted by date. If no filters are given, returns every approved event.</returns>
        [HttpGet]
        public IActionResult GetEvents([FromQuery] int? year, [FromQuery] int? month, [FromQuery] DateTime? date)
        {
            List<Event> matchingEvents = [];

            foreach (Event e in _context.Events)
            {
                if (e.ApprovedAt == null)
                {
                    continue;
                }

                if (date != null && e.EventDate.Date == date.Value.Date)
                {
                    matchingEvents.Add(e);
                }
                else if (date == null && year != null && month != null && e.EventDate.Year == year && e.EventDate.Month == month)
                {
                    matchingEvents.Add(e);
                }
                else if (date == null && year == null && month == null)
                {
                    matchingEvents.Add(e);
                }
            }

            matchingEvents.Sort((a, b) => a.EventDate.CompareTo(b.EventDate));

            List<EventResponse> response = [];

            foreach (Event e in matchingEvents)
            {
                response.Add(ToEventResponse(e));
            }

            return Ok(response);
        }

        /// <summary>
        /// Gets all events waiting for approval. Approvers only.
        /// </summary>
        /// <returns>The pending events, sorted by date.</returns>
        [HttpGet("pending")]
        public IActionResult GetPendingEvents()
        {
            if (!IsApprover())
            {
                return Forbid();
            }

            List<Event> pendingEvents = [];

            foreach (Event e in _context.Events)
            {
                if (e.ApprovedAt == null)
                {
                    pendingEvents.Add(e);
                }
            }

            pendingEvents.Sort((a, b) => a.EventDate.CompareTo(b.EventDate));

            return Ok(pendingEvents);
        }

        /// <summary>
        /// Submits a new event. Approvers have it approved automatically; normal users have it saved as pending.
        /// </summary>
        /// <param name="request">The event details being submitted.</param>
        /// <returns>The newly created event, or an error if the day is already full.</returns>
        [HttpPost]
        public IActionResult CreateEvent(CreateEventRequest request)
        {
            bool isApprover = IsApprover();

            if (isApprover && ApprovedCountForDay(request.EventDate) >= MaxApprovedPerDay())
            {
                return BadRequest($"This day already has the maximum of {MaxApprovedPerDay()} approved events.");
            }

            Event newEvent = new()
            {
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                EventDate = request.EventDate,
                EventTime = request.EventTime,
                CreatedByUserId = CurrentUserId(),
                CreatedAt = DateTime.UtcNow
            };

            if (isApprover)
            {
                newEvent.ApproverUserId = CurrentUserId();
                newEvent.ApprovedAt = DateTime.UtcNow;
            }

            _context.Events.Add(newEvent);
            _context.SaveChanges();

            return Ok(newEvent);
        }

        /// <summary>
        /// Approves a pending event. Approvers only.
        /// </summary>
        /// <param name="id">The id of the event to approve.</param>
        /// <returns>The approved event, or an error if it was not found or the day is already full.</returns>
        [HttpPost("{id}/approve")]
        public IActionResult ApproveEvent(int id)
        {
            if (!IsApprover())
            {
                return Forbid();
            }

            Event? eventToApprove = _context.Events.FirstOrDefault(e => e.EventId == id);

            if (eventToApprove == null)
            {
                return NotFound();
            }

            if (ApprovedCountForDay(eventToApprove.EventDate) >= MaxApprovedPerDay())
            {
                return BadRequest($"This day already has the maximum of {MaxApprovedPerDay()} approved events.");
            }

            eventToApprove.ApproverUserId = CurrentUserId();
            eventToApprove.ApprovedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok(eventToApprove);
        }

        /// <summary>
        /// Edits an existing event. Approvers only.
        /// </summary>
        /// <param name="id">The id of the event to edit.</param>
        /// <param name="request">The new event details.</param>
        /// <returns>The updated event, or an error if it was not found.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateEvent(int id, CreateEventRequest request)
        {
            if (!IsApprover())
            {
                return Forbid();
            }

            Event? eventToUpdate = _context.Events.FirstOrDefault(e => e.EventId == id);

            if (eventToUpdate == null)
            {
                return NotFound();
            }

            eventToUpdate.Title = request.Title;
            eventToUpdate.Description = request.Description;
            eventToUpdate.Location = request.Location;
            eventToUpdate.EventDate = request.EventDate;
            eventToUpdate.EventTime = request.EventTime;

            _context.SaveChanges();

            return Ok(eventToUpdate);
        }

        /// <summary>
        /// Deletes an event, pending or approved. Approvers only.
        /// </summary>
        /// <param name="id">The id of the event to delete.</param>
        /// <returns>Success, or an error if the event was not found.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            if (!IsApprover())
            {
                return Forbid();
            }

            Event? eventToDelete = _context.Events.FirstOrDefault(e => e.EventId == id);

            if (eventToDelete == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventToDelete);
            _context.SaveChanges();

            return Ok();
        }
    }
}