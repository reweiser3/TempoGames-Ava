using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ava.Data;
using Ava.Data.Statistics;
using Ava.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Ava.Services
{
    /// <summary>
    /// Service for calculating game statistics for a user.
    /// </summary>
    public class GameStatsService : IGameStatsService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public GameStatsService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<int> GetTotalGamesPlayedAsync(string userId)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.Games.CountAsync(g => g.UserId == userId);
        }

        public async Task<int> GetTotalWinsAsync(string userId)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.Games.CountAsync(g => g.UserId == userId && g.IsWin);
        }

        public async Task<int> GetTotalLossesAsync(string userId)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return await context.Games.CountAsync(g => g.UserId == userId && !g.IsWin);
        }

        public async Task<OpponentStats> GetMostFrequentOpponentAsync(string userId)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var mostFrequentOpponentGroup = await context.Games
                .Where(g => g.UserId == userId)
                .GroupBy(g => g.OpponentId)
                .OrderByDescending(g => g.Count())
                .Select(g => new { OpponentId = g.Key, Count = g.Count() })
                .FirstOrDefaultAsync();

            if (mostFrequentOpponentGroup == null) return null;

            var opponent = await context.Users.FindAsync(mostFrequentOpponentGroup.OpponentId);
            var totalGamesAgainstOpponent = mostFrequentOpponentGroup.Count;
            var winsAgainstOpponent = await context.Games
                .Where(g => g.UserId == userId && g.OpponentId == mostFrequentOpponentGroup.OpponentId && g.IsWin)
                .CountAsync();
            var lossesAgainstOpponent = totalGamesAgainstOpponent - winsAgainstOpponent;
            var lastPlayed = await context.Games
                .Where(g => g.UserId == userId && g.OpponentId == mostFrequentOpponentGroup.OpponentId)
                .OrderByDescending(g => g.DatePlayed)
                .Select(g => g.DatePlayed)
                .FirstOrDefaultAsync();

            return new OpponentStats
            {
                Opponent = opponent,
                Percentage = Math.Round(100 * (double)winsAgainstOpponent / totalGamesAgainstOpponent, 2),
                Wins = winsAgainstOpponent,
                Losses = lossesAgainstOpponent,
                LastPlayed = lastPlayed
            };
        }

        public async Task<OpponentStats> GetEasiestOpponentAsync(string userId)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var easiestOpponentGroup = await context.Games
                .Where(g => g.UserId == userId && g.IsWin)
                .GroupBy(g => g.OpponentId)
                .OrderByDescending(g => g.Count())
                .Select(g => new { OpponentId = g.Key, Wins = g.Count() })
                .FirstOrDefaultAsync();

            if (easiestOpponentGroup == null) return null;

            var opponent = await context.Users.FindAsync(easiestOpponentGroup.OpponentId);
            var totalGamesAgainstOpponent = await context.Games
                .Where(g => g.UserId == userId && g.OpponentId == easiestOpponentGroup.OpponentId)
                .CountAsync();
            var lossesAgainstOpponent = totalGamesAgainstOpponent - easiestOpponentGroup.Wins;
            var lastPlayed = await context.Games
                .Where(g => g.UserId == userId && g.OpponentId == easiestOpponentGroup.OpponentId)
                .OrderByDescending(g => g.DatePlayed)
                .Select(g => g.DatePlayed)
                .FirstOrDefaultAsync();

            return new OpponentStats
            {
                Opponent = opponent,
                Percentage = Math.Round(100 * (double)easiestOpponentGroup.Wins / totalGamesAgainstOpponent, 2),
                Wins = easiestOpponentGroup.Wins,
                Losses = lossesAgainstOpponent,
                LastPlayed = lastPlayed
            };
        }

        public async Task<OpponentStats> GetMostDifficultOpponentAsync(string userId)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var mostDifficultOpponentGroup = await context.Games
                .Where(g => g.UserId == userId && !g.IsWin)
                .GroupBy(g => g.OpponentId)
                .OrderByDescending(g => g.Count())
                .Select(g => new { OpponentId = g.Key, Losses = g.Count() })
                .FirstOrDefaultAsync();

            if (mostDifficultOpponentGroup == null) return null;

            var opponent = await context.Users.FindAsync(mostDifficultOpponentGroup.OpponentId);
            var totalGamesAgainstOpponent = await context.Games
                .Where(g => g.UserId == userId && g.OpponentId == mostDifficultOpponentGroup.OpponentId)
                .CountAsync();
            var winsAgainstOpponent = totalGamesAgainstOpponent - mostDifficultOpponentGroup.Losses;
            var lastPlayed = await context.Games
                .Where(g => g.UserId == userId && g.OpponentId == mostDifficultOpponentGroup.OpponentId)
                .OrderByDescending(g => g.DatePlayed)
                .Select(g => g.DatePlayed)
                .FirstOrDefaultAsync();

            return new OpponentStats
            {
                Opponent = opponent,
                Percentage = Math.Round(100 * (double)winsAgainstOpponent / totalGamesAgainstOpponent, 2),
                Wins = winsAgainstOpponent,
                Losses = mostDifficultOpponentGroup.Losses,
                LastPlayed = lastPlayed
            };
        }
    }
}
