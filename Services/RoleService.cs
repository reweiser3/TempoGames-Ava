using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ava.Data;
using Ava.Data.Users;
using Ava.Interfaces;

namespace Ava.Services
{
    /// <summary>
    /// Provides CRUD operations for roles.
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="userManager">The user manager.</param>
        public RoleService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Retrieves all roles with user counts, sorted by role name.
        /// </summary>
        /// <returns>A list of roles with user counts.</returns>
        public async Task<IEnumerable<RoleWithUserCount>> GetRolesAsync()
        {
            var roles = await _context.Roles.OrderBy(r => r.Name).ToListAsync();
            var rolesWithUserCounts = new List<RoleWithUserCount>();

            foreach (var role in roles)
            {
                var userCount = await _userManager.GetUsersInRoleAsync(role.Name);
                rolesWithUserCounts.Add(new RoleWithUserCount
                {
                    Role = role,
                    UserCount = userCount.Count
                });
            }

            return rolesWithUserCounts;
        }

        /// <summary>
        /// Retrieves a role by its identifier.
        /// </summary>
        /// <param name="id">The role identifier.</param>
        /// <returns>The role if found; otherwise, <c>null</c>.</returns>
        public async Task<IdentityRole> GetRoleByIdAsync(string id)
        {
            return await _context.Roles.FindAsync(id);
        }

        /// <summary>
        /// Adds a new role.
        /// </summary>
        /// <param name="role">The role to add.</param>
        /// <returns>The added role.</returns>
        public async Task<IdentityRole> AddRoleAsync(IdentityRole role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        /// <param name="role">The role to update.</param>
        /// <returns>The updated role.</returns>
        public async Task<IdentityRole> UpdateRoleAsync(IdentityRole role)
        {
            _context.Entry(role).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return role;
        }

        /// <summary>
        /// Deletes a role by its identifier.
        /// </summary>
        /// <param name="id">The role identifier.</param>
        /// <returns><c>true</c> if the role was deleted; otherwise, <c>false</c>.</returns>
        public async Task<bool> DeleteRoleAsync(string id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return false;
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            foreach (var user in usersInRole)
            {
                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gets all users in a specified role.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>A data transfer object containing the role and a list of users in the specified role.</returns>
        public async Task<RoleDescriptionWithUsers> GetUsersInRoleAsync(string roleName)
        {
            try
            {
                var users = await _userManager.GetUsersInRoleAsync(roleName) as List<ApplicationUser>;
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

                return new RoleDescriptionWithUsers
                {
                    Role = role,
                    Users = users
                };
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw;
            }
        }
    }
}
