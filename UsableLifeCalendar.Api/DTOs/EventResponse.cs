namespace UsableLifeCalendar.Api.DTOs
{
    public class EventResponse
    {
        public int EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateTime EventDate { get; set; }
        public TimeOnly EventTime { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByFirstName { get; set; } = string.Empty;
        public string? CreatedByMiddleName { get; set; }
        public string CreatedByLastName { get; set; } = string.Empty;
        public int? ApproverUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}
