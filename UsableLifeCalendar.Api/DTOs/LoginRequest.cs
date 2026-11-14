namespace UsableLifeCalendar.Api.DTOs
{
    public class LoginRequest
    {
        public string LogonName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}