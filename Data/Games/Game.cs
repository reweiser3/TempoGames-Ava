using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ava.Data.Games
{
    /// <summary>
    /// Represents a game played by a user.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Gets or sets the unique identifier for the game.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameId { get; set; }

        /// <summary>
        /// Gets or sets the user ID of the player who played the game.
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Navigation property for the user who played the game.
        /// </summary>
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game was won by the user.
        /// </summary>
        public bool IsWin { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the game was played.
        /// </summary>
        public DateTime DatePlayed { get; set; }
    }
}