namespace Ava.Data.Statistics
{
    /// <summary>
    /// Represents the opponent statistics including the user and win/loss percentage.
    /// </summary>
    public class OpponentStats
    {
        /// <summary>
        /// Gets or sets the opponent user.
        /// </summary>
        public ApplicationUser Opponent { get; set; }

        /// <summary>
        /// Gets or sets the win/loss percentage against the opponent.
        /// </summary>
        public double Percentage { get; set; }

        /// <summary>
        /// Gets or sets the number of wins against the opponent.
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// Gets or sets the number of losses against the opponent.
        /// </summary>
        public int Losses { get; set; }

        /// <summary>
        /// Gets or sets the date of the last time played against the opponent.
        /// </summary>
        public DateTime LastPlayed { get; set; }
    }
}