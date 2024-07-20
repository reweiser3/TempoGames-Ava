using Ava.Data;
using Ava.Data.Friendship;
using Microsoft.EntityFrameworkCore;

namespace Ava.Services
{
    public class FriendsService
    {
        private readonly ApplicationDbContext _context;

        public FriendsService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a friend to the user's friend list.
        /// </summary>
        /// <param name="primaryUserId">The ID of the user adding the friend.</param>
        /// <param name="friendUserId">The ID of the friend being added.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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
        /// Confirms a friendship if the friend has already added the primary user.
        /// </summary>
        /// <param name="primaryUserId">The ID of the primary user.</param>
        /// <param name="friendUserId">The ID of the friend.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown if the friend has not added the primary user.</exception>
        public async Task ConfirmFriendAsync(string primaryUserId, string friendUserId)
        {
            var friendship = await _context.Friends
                .FirstOrDefaultAsync(f => f.PrimaryUserId == friendUserId && f.FriendUserId == primaryUserId);

            if (friendship != null)
            {
                // The friend has already added the primary user, now we just need to confirm the friendship
                await AddFriendAsync(primaryUserId, friendUserId);
            }
            else
            {
                // The friend has not added the primary user yet
                throw new Exception("Friendship cannot be confirmed because the friend has not added the primary user.");
            }
        }

        /// <summary>
        /// Sets the favorite status for a friendship.
        /// </summary>
        /// <param name="primaryUserId">The ID of the primary user.</param>
        /// <param name="friendUserId">The ID of the friend.</param>
        /// <param name="isFavorite">The favorite status to set.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown if the friendship is not found.</exception>
        public async Task SetFavoriteAsync(string primaryUserId, string friendUserId, bool isFavorite)
        {
            var friendship = await _context.Friends
                .FirstOrDefaultAsync(f => f.PrimaryUserId == primaryUserId && f.FriendUserId == friendUserId);

            if (friendship != null)
            {
                friendship.IsFavorite = isFavorite;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Friendship not found.");
            }
        }
    }

}
