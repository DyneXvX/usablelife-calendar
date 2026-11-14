using System.ComponentModel.DataAnnotations;

namespace UsableLifeCalendar.Api.Models
{
    /*
     *  [RegularExpression(@"^[\x20-\x7E]+$")] - only normal printable characters.
     *  No tabs, line breaks, or control characters
    */

    public class User
    {
        public int UserId { get; set; }

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

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public bool IsApprover { get; set; }
    }
}