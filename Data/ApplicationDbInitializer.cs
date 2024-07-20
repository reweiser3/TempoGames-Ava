using Ava.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
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
            var friendsService = scope.ServiceProvider.GetRequiredService<FriendsService>();

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
            string defaultUserName = "demo@tempogames.com";
            string defaultUserPassword = "yourStrong@Password1";
            ApplicationUser defaultUser = null;
            if (await userManager.FindByNameAsync(defaultUserName) == null)
            {
                defaultUser = new ApplicationUser
                {
                    UserName = defaultUserName,
                    Email = defaultUserName,
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
                var result = await userManager.CreateAsync(defaultUser, defaultUserPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, roleName);
                }
            }
            else
            {
                defaultUser = await userManager.FindByNameAsync(defaultUserName);
            }

            // Create 10 fictional users
            var users = new[]
            {
                new { UserName = "john.doe@example.com", Password = "Password123!", Given = "John", Middle = "A", Family = "Doe", Birthdate = new DateTime(1990, 1, 1), Gamertag = "JohnnyD", Gender = "Male", City = "New York", State = "New York", Country = "United States" },
                new { UserName = "jane.smith@example.com", Password = "Password123!", Given = "Jane", Middle = "B", Family = "Smith", Birthdate = new DateTime(1992, 2, 2), Gamertag = "JaneS", Gender = "Female", City = "Los Angeles", State = "California", Country = "United States" },
                new { UserName = "bob.jones@example.com", Password = "Password123!", Given = "Bob", Middle = "C", Family = "Jones", Birthdate = new DateTime(1985, 3, 3), Gamertag = "BobbyJ", Gender = "Male", City = "Chicago", State = "Illinois", Country = "United States" },
                new { UserName = "alice.williams@example.com", Password = "Password123!", Given = "Alice", Middle = "D", Family = "Williams", Birthdate = new DateTime(1988, 4, 4), Gamertag = "AliceW", Gender = "Female", City = "Houston", State = "Texas", Country = "United States" },
                new { UserName = "charlie.brown@example.com", Password = "Password123!", Given = "Charlie", Middle = "E", Family = "Brown", Birthdate = new DateTime(1995, 5, 5), Gamertag = "CharlieB", Gender = "Male", City = "Phoenix", State = "Arizona", Country = "United States" },
                new { UserName = "david.davis@example.com", Password = "Password123!", Given = "David", Middle = "F", Family = "Davis", Birthdate = new DateTime(1980, 6, 6), Gamertag = "DaveD", Gender = "Male", City = "Philadelphia", State = "Pennsylvania", Country = "United States" },
                new { UserName = "emily.miller@example.com", Password = "Password123!", Given = "Emily", Middle = "G", Family = "Miller", Birthdate = new DateTime(1998, 7, 7), Gamertag = "EmmyM", Gender = "Female", City = "San Antonio", State = "Texas", Country = "United States" },
                new { UserName = "frank.moore@example.com", Password = "Password123!", Given = "Frank", Middle = "H", Family = "Moore", Birthdate = new DateTime(1991, 8, 8), Gamertag = "FrankM", Gender = "Male", City = "San Diego", State = "California", Country = "United States" },
                new { UserName = "grace.taylor@example.com", Password = "Password123!", Given = "Grace", Middle = "I", Family = "Taylor", Birthdate = new DateTime(1987, 9, 9), Gamertag = "GracieT", Gender = "Female", City = "Dallas", State = "Texas", Country = "United States" },
                new { UserName = "henry.anderson@example.com", Password = "Password123!", Given = "Henry", Middle = "J", Family = "Anderson", Birthdate = new DateTime(1983, 10, 10), Gamertag = "HenryA", Gender = "Male", City = "San Jose", State = "California", Country = "United States" },
            };

            var userIds = new List<string>();
            foreach (var u in users)
            {
                if (await userManager.FindByNameAsync(u.UserName) == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = u.UserName,
                        Email = u.UserName,
                        EmailConfirmed = true,
                        Given = u.Given,
                        Middle = u.Middle,
                        Family = u.Family,
                        Birthdate = u.Birthdate,
                        Gamertag = u.Gamertag,
                        Gender = u.Gender,
                        City = u.City,
                        State = u.State,
                        Country = u.Country
                    };

                    var result = await userManager.CreateAsync(user, u.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                        userIds.Add(user.Id);
                    }
                }
                else
                {
                    var existingUser = await userManager.FindByNameAsync(u.UserName);
                    userIds.Add(existingUser.Id);
                }
            }

            // Randomly select 5 users to add as friends to the default user
            var random = new Random();
            var selectedFriends = userIds.OrderBy(x => random.Next()).Take(5).ToList();
            foreach (var friendId in selectedFriends)
            {
                await friendsService.AddFriendAsync(defaultUser.Id, friendId);
            }
        }
    }
}