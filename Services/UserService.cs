﻿using Ava.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ava.Data.Users;

public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RoleManager<IdentityRole> _roleManager;
    public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger, IServiceScopeFactory scopeFactory, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _logger = logger;
        _scopeFactory = scopeFactory;
        _roleManager = roleManager;
    }

    /// <summary>
    /// Finds a user by their ID asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user to find.</param>
    /// <returns>The found ApplicationUser or null if not found.</returns>
    /// <exception cref="Exception">Throws an exception if an error occurs while finding the user.</exception>
    public async Task<ApplicationUser> FindUserByIdAsync(string userId)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try
            {
                return await userManager.FindByIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while finding the user by ID.");
                throw;
            }
        }
    }

    /// <summary>
    /// Updates the details of an existing user asynchronously based on ApplicationUserInputModel.
    /// </summary>
    /// <param name="userInput">The user input model with updated details.</param>
    /// <param name="userId">The ID of the user to update.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
    /// <exception cref="Exception">Throws an exception if an error occurs while updating the user.</exception>
    public async Task<bool> UpdateUserAsync(ApplicationUserInputModel userInput, string userId)
    {
        try
        {
            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null)
            {
                _logger.LogError("User with ID {UserId} not found.", userId);
                return false;
            }

            // Update the existing user's properties
            existingUser.Given = userInput.Given;
            existingUser.Middle = userInput.Middle;
            existingUser.Family = userInput.Family;
            existingUser.Birthdate = userInput.Birthdate;
            existingUser.Gamertag = userInput.Gamertag;
            existingUser.Gender = userInput.Gender;
            existingUser.City = userInput.City;
            existingUser.State = userInput.State;
            existingUser.Country = userInput.Country;
            existingUser.ProfilePictureUrl = userInput.ProfilePictureUrl;

            var result = await _userManager.UpdateAsync(existingUser);
            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} was successfully updated.", userId);
                return true;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Error updating user with ID {UserId}: {ErrorDescription}", userId, error.Description);
                }
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the user with ID {UserId}.", userId);
            throw;
        }
    }

    /// <summary>
    /// Gets all users asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with a list of ApplicationUser as the result.</returns>
    /// <exception cref="Exception">Throws an exception if an error occurs while getting all users.</exception>
    public async Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try
            {
                return await userManager.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all users.");
                throw;
            }
        }
    }

    /// <summary>
    /// Gets the user's friends asynchronously.
    /// </summary>
    /// <param name="userId">The user id we are search for friends</param>
    /// <returns>a list of application users that are friends with the selected user</returns>
    public async Task<List<ApplicationUser>> GetFriendsAsync(string userId)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            try
            {
                var user = await userManager.Users.Include(u => u.Friendships)
                    .ThenInclude(f => f.FriendUser)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                return user?.Friendships.Select(f => f.FriendUser).ToList() ?? new List<ApplicationUser>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting friends for user ID {UserId}.", userId);
                throw;
            }
        }
    }

    /// <summary>
    /// Sets the profile picture URL for a user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="profilePictureUrl">The new profile picture URL.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
    /// <exception cref="Exception">Throws an exception if an error occurs while updating the user.</exception>
    public async Task<bool> SetUserProfilePictureAsync(string userId, string profilePictureFileName)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("User with ID {UserId} not found.", userId);
                return false;
            }

            user.ProfilePictureUrl = profilePictureFileName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("Profile picture for user with ID {UserId} was successfully updated.", userId);
                return true;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Error updating profile picture for user with ID {UserId}: {ErrorDescription}", userId, error.Description);
                }
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the profile picture for user with ID {UserId}.", userId);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all roles asynchronously.
    /// </summary>
    /// <returns>A list of IdentityRole objects.</returns>
    public async Task<List<IdentityRole>> GetAllRolesAsync()
    {
        return await _roleManager.Roles.ToListAsync();
    }

    /// <summary>
    /// Checks if the user is in a role.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="roleName">The role name.</param>
    /// <returns>True if the user is in the role, otherwise false.</returns>
    public async Task<bool> IsUserInRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null && await _userManager.IsInRoleAsync(user, roleName);
    }

    /// <summary>
    /// Add the user to the role
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task AddToRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
    }

    /// <summary>
    /// Removes the user from the selected role
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task RemoveFromRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }
    }
}
