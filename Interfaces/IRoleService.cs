using Ava.Data;
using Ava.Data.Users;
using Microsoft.AspNetCore.Identity;

namespace Ava.Interfaces
{
    /// <summary>
    /// Defines the contract for a role service.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Retrieves all roles with user counts.
        /// </summary>
        /// <returns>A list of roles with user counts.</returns>
        Task<IEnumerable<RoleWithUserCount>> GetRolesAsync();

        /// <summary>
        /// Retrieves a role by its identifier.
        /// </summary>
        /// <param name="id">The role identifier.</param>
        /// <returns>The role if found; otherwise, <c>null</c>.</returns>
        Task<IdentityRole> GetRoleByIdAsync(string id);

        /// <summary>
        /// Adds a new role.
        /// </summary>
        /// <param name="role">The role to add.</param>
        /// <returns>The added role.</returns>
        Task<IdentityRole> AddRoleAsync(IdentityRole role);

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        /// <param name="role">The role to update.</param>
        /// <returns>The updated role.</returns>
        Task<IdentityRole> UpdateRoleAsync(IdentityRole role);

        /// <summary>
        /// Deletes a role by its identifier.
        /// </summary>
        /// <param name="id">The role identifier.</param>
        /// <returns><c>true</c> if the role was deleted; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteRoleAsync(string id);

        /// <summary>
        /// Gets all users in a specified role.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>A data transfer object containing the role and a list of users in the specified role.</returns>
        Task<RoleDescriptionWithUsers> GetUsersInRoleAsync(string roleName);
    }
}
