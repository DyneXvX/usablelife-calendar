namespace UsableLifeCalendar.Api.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public bool IsApprover { get; set; }
    }
}
