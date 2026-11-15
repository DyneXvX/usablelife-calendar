using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsableLifeCalendar.Api.Data;
using UsableLifeCalendar.Api.DTOs;
using UsableLifeCalendar.Api.Models;
using UsableLifeCalendar.Api.Services;

namespace UsableLifeCalendar.Api.Controllers
{
    /// <summary>
    /// Handles managing user accounts. Approvers only.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Creates the controller with the database it needs.
        /// </summary>
        /// <param name="context">The database connection.</param>
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Checks whether the logged-in user is an approver.
        /// </summary>
        /// <returns>True if the logged-in user is an approver, false if not.</returns>
        private bool IsApprover()
        {
            string isApproverText = User.FindFirst("IsApprover")!.Value;
            return isApproverText == "True";
        }

        /// <summary>
        /// Builds the safe version of a user to send back to the client - leaves out the password hash.
        /// </summary>
        /// <param name="user">The full user record from the database.</param>
        /// <returns>The user's details, without the password hash.</returns>
        private UserResponse ToUserResponse(User user)
        {
            UserResponse response = new()
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                LogonName = user.LogonName,
                IsApprover = user.IsApprover
            };
            return response;
        }

        /// <summary>
        /// Gets every user. Approvers only.
        /// </summary>
        /// <returns>The list of all users.</returns>
        [HttpGet]
        public IActionResult GetUsers()
        {
            if (!IsApprover())
            {
                return Forbid();
            }

            List<UserResponse> userResponses = new List<UserResponse>();

            foreach (User u in _context.Users)
            {
                userResponses.Add(ToUserResponse(u));
            }

            return Ok(userResponses);
        }

        /// <summary>
        /// Creates a new user. Approvers only.
        /// </summary>
        /// <param name="request">The new user's details, including a plain-text password to hash.</param>
        /// <returns>The newly created user.</returns>
        [HttpPost]
        public IActionResult CreateUser(CreateUserRequest request)
        {
            if (!IsApprover())
            {
                return Forbid();
            }

            User newUser = new User
            {
                FirstName = request.FirstName.Trim(),
                MiddleName = request.MiddleName?.Trim(),
                LastName = request.LastName.Trim(),
                LogonName = request.LogonName.Trim(),
                PasswordHash = PasswordHasher.Hash(request.Password),
                IsApprover = request.IsApprover
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(ToUserResponse(newUser));
        }

        /// <summary>
        /// Edits an existing user. Approvers only. Does not change the password.
        /// </summary>
        /// <param name="id">The id of the user to edit.</param>
        /// <param name="request">The new user details.</param>
        /// <returns>The updated user, or an error if it was not found.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UpdateUserRequest request)
        {
            if (!IsApprover())
            {
                return Forbid();
            }

            User? user = _context.Users.FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = request.FirstName.Trim();
            user.MiddleName = request.MiddleName?.Trim();
            user.LastName = request.LastName.Trim();
            user.LogonName = request.LogonName.Trim();
            user.IsApprover = request.IsApprover;

            _context.SaveChanges();

            return Ok(ToUserResponse(user));
        }

        /// <summary>
        /// Deletes a user. Approvers only.
        /// </summary>
        /// <param name="id">The id of the user to delete.</param>
        /// <returns>Success, or an error if the user was not found.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            if (!IsApprover())
            {
                return Forbid();
            }

            User? user = _context.Users.FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok();
        }
    }
}