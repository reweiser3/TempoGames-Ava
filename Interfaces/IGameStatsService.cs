namespace Ava.Interfaces
{
    /// <summary>
    /// Interface for game stats service to calculate user game statistics.
    /// </summary>
    public interface IGameStatsService
    {
        Task<int> GetTotalGamesPlayedAsync(string userId);
        Task<int> GetTotalWinsAsync(string userId);
        Task<int> GetTotalLossesAsync(string userId);
    }
}
