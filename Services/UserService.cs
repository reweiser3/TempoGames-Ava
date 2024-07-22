using Ava.Data;
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

    public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger, IServiceScopeFactory scopeFactory)
    {
        _userManager = userManager;
        _logger = logger;
        _scopeFactory = scopeFactory;
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
}
