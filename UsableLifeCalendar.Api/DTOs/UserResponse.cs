namespace UsableLifeCalendar.Api.DTOs
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string LogonName { get; set; } = string.Empty;
        public bool IsApprover { get; set; }
    }
}
