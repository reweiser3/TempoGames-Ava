using Ava.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
    /// Updates the details of an existing user asynchronously.
    /// </summary>
    /// <param name="user">The user object with updated details.</param>
    /// <returns>A task representing the asynchronous operation, with a boolean result indicating success.</returns>
    /// <exception cref="Exception">Throws an exception if an error occurs while updating the user.</exception>
    public async Task<bool> UpdateUserAsync(ApplicationUser user)
    {
        try
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null)
            {
                _logger.LogError("User with ID {UserId} not found.", user.Id);
                return false;
            }

            // Update the existing user's properties
            existingUser.PhoneNumber = user.PhoneNumber;

            var result = await _userManager.UpdateAsync(existingUser);
            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} was successfully updated.", user.Id);
                return true;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Error updating user with ID {UserId}: {ErrorDescription}", user.Id, error.Description);
                }
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the user with ID {UserId}.", user.Id);
            throw;
        }
    }
}
