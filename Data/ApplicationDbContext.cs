using Ava.Data.Friendship;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ava.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define the Friends DbSet
        public DbSet<Friend> Friends { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationships for the Friendship entity
            modelBuilder.Entity<Friend>()
                .HasOne(f => f.PrimaryUser)
                .WithMany(u => u.Friendships)
                .HasForeignKey(f => f.PrimaryUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.FriendUser)
                .WithMany(u => u.FriendsOf)
                .HasForeignKey(f => f.FriendUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}