// Ignore Spelling: Hasher

using BCrypt.Net;

namespace UsableLifeCalendar.Api.Services
{
    /// <summary>
    /// Hashes and checks passwords using BCrypt.
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Hashes a plain-text password.
        /// </summary>
        /// <param name="password">The plain-text password.</param>
        /// <returns>The hashed password, safe to store in the database.</returns>
        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Checks a plain-text password against a stored hash.
        /// </summary>
        /// <param name="password">The plain-text password someone typed in.</param>
        /// <param name="passwordHash">The hashed password stored in the database.</param>
        /// <returns>True if the password matches the hash, false if not.</returns>
        public static bool Verify(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
