namespace Ava.Data.Friendship
{
    public class FriendWithLastPlayed
    {
        public ApplicationUser FriendUser { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime? LastPlayed { get; set; }
    }
}