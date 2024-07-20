using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ava.Data.Friendship
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string PrimaryUserId { get; set; }

        [ForeignKey(nameof(PrimaryUserId))]
        public virtual ApplicationUser PrimaryUser { get; set; }

        [Required]
        public string FriendUserId { get; set; }

        [ForeignKey(nameof(FriendUserId))]
        public virtual ApplicationUser FriendUser { get; set; }

        public bool IsFavorite { get; set; }
    }

}
