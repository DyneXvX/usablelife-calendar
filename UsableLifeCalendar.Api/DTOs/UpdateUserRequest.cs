using System.ComponentModel.DataAnnotations;

namespace UsableLifeCalendar.Api.DTOs
{
    public class UpdateUserRequest
    {
        [Required]
        [StringLength(25)]
        [RegularExpression(@"^[\x20-\x7E]+$")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(25)]
        [RegularExpression(@"^[\x20-\x7E]*$")]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(25)]
        [RegularExpression(@"^[\x20-\x7E]+$")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(16, MinimumLength = 4)]
        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public string LogonName { get; set; } = string.Empty;

        public bool IsApprover { get; set; }
    }
}
