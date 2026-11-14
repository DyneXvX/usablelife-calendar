using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsableLifeCalendar.Api.Data;
using UsableLifeCalendar.Api.DTOs;
using UsableLifeCalendar.Api.Services;

namespace UsableLifeCalendar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

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

            // This just proves the check works for now. The real JWT token gets
            // added here in the next task.
            return Ok("Login successful for " + user.LogonName);
        }
    }
}