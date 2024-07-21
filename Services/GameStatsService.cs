using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ava.Data;
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
    }
}