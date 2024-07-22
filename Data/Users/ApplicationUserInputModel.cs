using System.ComponentModel.DataAnnotations;

namespace Ava.Data.Users
{
    /// <summary>
    /// Represents the input model for user data.
    /// </summary>
    public class ApplicationUserInputModel
    {
        /// <summary>
        /// The given (first) name of the user.
        /// This field is required.
        /// </summary>
        [Required]
        public string Given { get; set; }

        /// <summary>
        /// The middle name of the user.
        /// This field is optional.
        /// </summary>
        public string Middle { get; set; }

        /// <summary>
        /// The family (last) name of the user.
        /// This field is required.
        /// </summary>
        [Required]
        public string Family { get; set; }

        /// <summary>
        /// The birthdate of the user.
        /// This field is required.
        /// </summary>
        [Required]
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// The unique gamertag of the user.
        /// Must consist of letters, numbers, underscores (_), or hyphens (-) only.
        /// This field is required and must be unique.
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$",
            ErrorMessage = "Gamertag must consist of letters, numbers, _, or - only.")]
        public string Gamertag { get; set; }

        /// <summary>
        /// The gender of the user.
        /// This field is optional.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// The city of residence of the user.
        /// This field is optional.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The state of residence of the user.
        /// This field is optional.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The country of residence of the user.
        /// This field is required.
        /// </summary>
        [Required]
        public string Country { get; set; }

        /// <summary>
        /// The profile picture URL of the user.
        /// This field is optional.
        /// </summary>
        public string? ProfilePictureUrl { get; set; }

    }
}
