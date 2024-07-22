using Ava.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ava.Data.Games;

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
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure database is created
            await context.Database.MigrateAsync();

            // Create default roles
            string playerRoleName = "Player";
            string adminRoleName = "Admin";

            if (!await roleManager.RoleExistsAsync(playerRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(playerRoleName));
            }

            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRoleName));
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
                    Country = "United States",
                    ProfilePictureUrl = "default.png"
                };
                var result = await userManager.CreateAsync(defaultUser, defaultUserPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, playerRoleName);
                    await userManager.AddToRoleAsync(defaultUser, adminRoleName);
                }
            }
            else
            {
                defaultUser = await userManager.FindByNameAsync(defaultUserName);
            }

            // Check if the default user already has friends
            if (defaultUser != null && !context.Friends.Any(f => f.PrimaryUserId == defaultUser.Id))
            {
                // Create 10 fictional users
                var users = new[]
                {
                    new { UserName = "john.doe@example.com", ProfilePictureUrl = "profile-picture-m-1.jpg", Password = "Password123!", Given = "John", Middle = "A", Family = "Doe", Birthdate = new DateTime(1990, 1, 1), Gamertag = "JohnnyD", Gender = "Male", City = "New York", State = "New York", Country = "United States" },
                    new { UserName = "jane.smith@example.com", ProfilePictureUrl = "profile-picture-f-1.jpg", Password = "Password123!", Given = "Jane", Middle = "B", Family = "Smith", Birthdate = new DateTime(1992, 2, 2), Gamertag = "JaneS", Gender = "Female", City = "Los Angeles", State = "California", Country = "United States" },
                    new { UserName = "bob.jones@example.com", ProfilePictureUrl = "profile-picture-m-2.jpg", Password = "Password123!", Given = "Bob", Middle = "C", Family = "Jones", Birthdate = new DateTime(1985, 3, 3), Gamertag = "BobbyJ", Gender = "Male", City = "Chicago", State = "Illinois", Country = "United States" },
                    new { UserName = "alice.williams@example.com", ProfilePictureUrl = "profile-picture-f-2.jpg", Password = "Password123!", Given = "Alice", Middle = "D", Family = "Williams", Birthdate = new DateTime(1988, 4, 4), Gamertag = "AliceW", Gender = "Female", City = "Houston", State = "Texas", Country = "United States" },
                    new { UserName = "charlie.brown@example.com", ProfilePictureUrl = "profile-picture-m-3.jpg", Password = "Password123!", Given = "Charlie", Middle = "E", Family = "Brown", Birthdate = new DateTime(1995, 5, 5), Gamertag = "CharlieB", Gender = "Male", City = "Phoenix", State = "Arizona", Country = "United States" },
                    new { UserName = "david.davis@example.com", ProfilePictureUrl = "profile-picture-m-4.jpg", Password = "Password123!", Given = "David", Middle = "F", Family = "Davis", Birthdate = new DateTime(1980, 6, 6), Gamertag = "DaveD", Gender = "Male", City = "Philadelphia", State = "Pennsylvania", Country = "United States" },
                    new { UserName = "emily.miller@example.com", ProfilePictureUrl = "profile-picture-f-3.jpg", Password = "Password123!", Given = "Emily", Middle = "G", Family = "Miller", Birthdate = new DateTime(1998, 7, 7), Gamertag = "EmmyM", Gender = "Female", City = "San Antonio", State = "Texas", Country = "United States" },
                    new { UserName = "frank.moore@example.com", ProfilePictureUrl = "profile-picture-m-5.jpg", Password = "Password123!", Given = "Frank", Middle = "H", Family = "Moore", Birthdate = new DateTime(1991, 8, 8), Gamertag = "FrankM", Gender = "Male", City = "San Diego", State = "California", Country = "United States" },
                    new { UserName = "grace.taylor@example.com", ProfilePictureUrl = "profile-picture-f-4.jpg", Password = "Password123!", Given = "Grace", Middle = "I", Family = "Taylor", Birthdate = new DateTime(1987, 9, 9), Gamertag = "GracieT", Gender = "Female", City = "Dallas", State = "Texas", Country = "United States" },
                    new { UserName = "henry.anderson@example.com", ProfilePictureUrl = "profile-picture-m-6.jpg", Password = "Password123!", Given = "Henry", Middle = "J", Family = "Anderson", Birthdate = new DateTime(1983, 10, 10), Gamertag = "HenryA", Gender = "Male", City = "San Jose", State = "California", Country = "United States" },
                };

                var userIds = new List<string>();
                var random = new Random();
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
                            Country = u.Country,
                            ProfilePictureUrl = u.ProfilePictureUrl
                        };

                        var result = await userManager.CreateAsync(user, u.Password);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, playerRoleName);
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
                var selectedFriends = userIds.OrderBy(x => random.Next()).Take(5).ToList();
                foreach (var friendId in selectedFriends)
                {
                    await friendsService.AddFriendAsync(defaultUser.Id, friendId);
                    await friendsService.AddFriendAsync(friendId, defaultUser.Id);
                }

                // Randomly assign friends to each user
                foreach (var userId in userIds)
                {
                    var numberOfFriends = random.Next(3, 8); // Random number of friends between 3 and 7
                    var friends = userIds
                        .Where(id => id != userId) // Exclude the user itself
                        .OrderBy(x => random.Next())
                        .Take(numberOfFriends)
                        .ToList();

                    foreach (var friendId in friends)
                    {
                        await friendsService.AddFriendAsync(userId, friendId);
                    }
                }

                // Insert odd number of games for each user, including the default user
                var allUsers = new List<string>(userIds) { defaultUser.Id };
                foreach (var userId in allUsers)
                {
                    var numberOfGames = random.Next(27, 50) | 1; // Random odd number of games between 27 and 49
                    var opponentWinCounts = new Dictionary<string, int>();
                    var opponentLossCounts = new Dictionary<string, int>();

                    for (int i = 0; i < numberOfGames; i++)
                    {
                        var opponentId = userIds[random.Next(userIds.Count)];
                        // Ensure opponent is not the same as the user
                        while (opponentId == userId)
                        {
                            opponentId = userIds[random.Next(userIds.Count)];
                        }

                        bool isWin = random.Next(0, 2) == 1;

                        // Adjust win/loss to avoid 50% win rate against the opponent
                        if (!opponentWinCounts.ContainsKey(opponentId))
                        {
                            opponentWinCounts[opponentId] = 0;
                            opponentLossCounts[opponentId] = 0;
                        }

                        if (opponentWinCounts[opponentId] == opponentLossCounts[opponentId])
                        {
                            isWin = random.Next(0, 2) == 0;
                        }

                        if (isWin)
                        {
                            opponentWinCounts[opponentId]++;
                        }
                        else
                        {
                            opponentLossCounts[opponentId]++;
                        }

                        var game = new Game
                        {
                            UserId = userId,
                            OpponentId = opponentId,
                            IsWin = isWin,
                            DatePlayed = DateTime.Now.AddDays(-random.Next(1, 365)) // Random date within the last year
                        };
                        await context.Games.AddAsync(game);
                    }
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
