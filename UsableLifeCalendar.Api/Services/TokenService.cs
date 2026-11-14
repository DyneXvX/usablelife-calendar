using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UsableLifeCalendar.Api.Models;

namespace UsableLifeCalendar.Api.Services
{
    /// <summary>
    /// Builds login tokens (JWTs).
    /// </summary>
    public static class TokenService
    {
        /// <summary>
        /// Builds a signed login token for a user.
        /// </summary>
        /// <param name="user">The user the token is for.</param>
        /// <param name="key">The secret key used to sign the token.</param>
        /// <param name="issuer">The token issuer.</param>
        /// <param name="audience">The token audience.</param>
        /// <param name="expiryMinutes">How many minutes until the token expires.</param>
        /// <returns>The signed token, as a string.</returns>
        public static string GenerateToken(User user, string key, string issuer, string audience, int expiryMinutes)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new("IsApprover", user.IsApprover.ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}