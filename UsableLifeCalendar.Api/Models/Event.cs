using System.ComponentModel.DataAnnotations;

namespace UsableLifeCalendar.Api.Models
{
    /*
     *  [RegularExpression(@"^[\x20-\x7E]+$")] - only normal printable characters.
     *  No tabs, line breaks, or control characters
    */

    public class Event
    {
        public int EventId { get; set; }

        [Required]
        [StringLength(25)]
        [RegularExpression(@"^[\x20-\x7E]+$")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [RegularExpression(@"^[\x20-\x7E]+$")]
        public string Description { get; set; } = string.Empty;

        [StringLength(50)]
        [RegularExpression(@"^[\x20-\x7E]*$")]
        public string? Location { get; set; }

        public DateTime EventDate { get; set; }
        public TimeOnly EventTime { get; set; }

        public int CreatedByUserId { get; set; }
        public int? ApproverUserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}