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
}