using System.ComponentModel.DataAnnotations;

namespace UsableLifeCalendar.Api.DTOs
{
    public class CreateEventRequest
    {
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
    }
}
