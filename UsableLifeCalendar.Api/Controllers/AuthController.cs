using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsableLifeCalendar.Api.Data;
using UsableLifeCalendar.Api.DTOs;
using UsableLifeCalendar.Api.Services;

namespace UsableLifeCalendar.Api.Controllers
{
    /// <summary>
    /// Handles signing in.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        /// <summary>
        /// Creates the controller with the database and app settings it needs.
        /// </summary>
        /// <param name="context">The database connection.</param>
        /// <param name="config">The app settings (used to read the JWT settings).</param>
        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Checks a logon name and password, and returns a login token if they are correct.
        /// </summary>
        /// <param name="request">The logon name and password.</param>
        /// <returns>A login token and the user's approver status, or an error if the logon name or password is wrong.</returns>
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.LogonName == request.LogonName);

            if (user == null)
            {
                return Unauthorized("Logon name or password is incorrect.");
            }

            var passwordIsCorrect = PasswordHasher.Verify(request.Password, user.PasswordHash);

            if (!passwordIsCorrect)
            {
                return Unauthorized("Logon name or password is incorrect.");
            }

            var token = TokenService.GenerateToken(
                user,
                _config["Jwt:Key"]!,
                _config["Jwt:Issuer"]!,
                _config["Jwt:Audience"]!,
                int.Parse(_config["Jwt:ExpiryMinutes"]!)
            );

            return Ok(new LoginResponse { Token = token, IsApprover = user.IsApprover });
        }
    }
}