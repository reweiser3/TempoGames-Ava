using Microsoft.AspNetCore.Identity;

namespace Ava.Data.Users
{
    /// <summary>
    /// Role Description With the Application User List
    /// </summary>
    public class RoleDescriptionWithUsers
    {
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public IdentityRole Role { get; set; }

        /// <summary>
        /// Gets or sets the list of users in the role.
        /// </summary>
        public List<ApplicationUser> Users { get; set; }
    }
}
