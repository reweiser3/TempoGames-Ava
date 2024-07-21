using Ava.Data;
using Ava.Data.Friendship;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Ava.Services
{
    public class FriendsService
    {
        private readonly ApplicationDbContext _context;

        public FriendsService(ApplicationDbContext context)
        {
            _context = context;
        }

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

        public async Task<List<Friend>> GetFriendsByUserIdAsync(string userId)
        {
            return await _context.Friends
                .Where(f => f.PrimaryUserId == userId)
                .Include(f => f.FriendUser)
                .ToListAsync();
        }

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
