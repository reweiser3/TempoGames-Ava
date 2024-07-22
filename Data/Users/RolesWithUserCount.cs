using Microsoft.AspNetCore.Identity;

namespace Ava.Data.Users
{
    /// <summary>
    /// Represents a role with a count of users in that role.
    /// </summary>
    public class RoleWithUserCount
    {
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public IdentityRole Role { get; set; }

        /// <summary>
        /// Gets or sets the count of users in the role.
        /// </summary>
        public int UserCount { get; set; }
    }
}
