using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Ava.Data
{
    public class ApplicationDbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Ensure database is created
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();

            // Create default role
            string roleName = "Player";
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Create default user
            string userName = "demo@tempogames.com";
            string userPassword = "yourStrong@Password1";
            if (await userManager.FindByNameAsync(userName) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true,
                    PhoneNumber = "+1 (336) 995-8440",
                    PhoneNumberConfirmed = true,
                    Given = "Richard",
                    Middle = "E",
                    Family = "Weiser",
                    Birthdate = new DateTime(1975, 11, 16),
                    Gamertag = "LocutusOfBorg",
                    Gender = "Male",
                    City = "Greensboro",
                    State = "North Carolina",
                    Country = "United States"
                };
                var result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}
