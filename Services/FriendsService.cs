using Ava.Data;
using Ava.Data.Friendship;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Ava.Services
{
    /// <summary>
    /// Provides services related to managing friends in the application.
    /// </summary>
    public class FriendsService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendsService"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public FriendsService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a friend for a specified primary user.
        /// </summary>
        /// <param name="primaryUserId">The ID of the primary user.</param>
        /// <param name="friendUserId">The ID of the friend to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddFriendAsync(string primaryUserId, string friendUserId)
        {
            var friend = new Friend
            {
                PrimaryUserId = primaryUserId,
                FriendUserId = friendUserId,
                IsFavorite = false
            };

            _context.Friends.Add(friend);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Confirms a friendship between two users.
        /// </summary>
        /// <param name="primaryUserId">The ID of the primary user.</param>
        /// <param name="friendUserId">The ID of the friend user.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown when the friend has not added the primary user.</exception>
        public async Task ConfirmFriendAsync(string primaryUserId, string friendUserId)
        {
            var friendship = await _context.Friends
                .FirstOrDefaultAsync(f => f.PrimaryUserId == friendUserId && f.FriendUserId == primaryUserId);

            if (friendship != null)
            {
                await AddFriendAsync(primaryUserId, friendUserId);
            }
            else
            {
                throw new Exception("Friendship cannot be confirmed because the friend has not added the primary user.");
            }
        }

        /// <summary>
        /// Toggles the favorite status of a friendship.
        /// </summary>
        /// <param name="primaryUserId">The ID of the primary user.</param>
        /// <param name="friendUserId">The ID of the friend user.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown when the friendship is not found.</exception>
        public async Task ToggleFavoriteAsync(string primaryUserId, string friendUserId)
        {
            var friendship = await _context.Friends
                .FirstOrDefaultAsync(f => f.PrimaryUserId == primaryUserId && f.FriendUserId == friendUserId);

            if (friendship != null)
            {
                friendship.IsFavorite = !friendship.IsFavorite;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Friendship not found.");
            }
        }

        /// <summary>
        /// Gets a list of friends for a specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of friends.</returns>
        public async Task<List<Friend>> GetFriendsByUserIdAsync(string userId)
        {
            return await _context.Friends
                .Where(f => f.PrimaryUserId == userId)
                .Include(f => f.FriendUser)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a list of friends with the last played date for a specified user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of friends with the last played date.</returns>
        public async Task<List<FriendWithLastPlayed>> GetFriendsWithLastPlayedByUserIdAsync(string userId)
        {
            var friends = await (from f in _context.Friends
                                 join u in _context.Users on f.FriendUserId equals u.Id
                                 where f.PrimaryUserId == userId
                                 select new FriendWithLastPlayed
                                 {
                                     FriendUser = u,
                                     IsFavorite = f.IsFavorite,
                                     LastPlayed = _context.Games
                                         .Where(g => (g.UserId == userId && g.OpponentId == f.FriendUserId) ||
                                                     (g.UserId == f.FriendUserId && g.OpponentId == userId))
                                         .OrderByDescending(g => g.DatePlayed)
                                         .Select(g => g.DatePlayed)
                                         .FirstOrDefault()
                                 }).ToListAsync();

            return friends;
        }
    }
}
