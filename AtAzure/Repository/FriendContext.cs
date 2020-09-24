using ApiFriends.Models;
using ApiFriends.Repository.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ApiFriends.Repository
{
    public class FriendContext : DbContext
    {
        public DbSet<Friend> Friends { set; get; }
        public FriendContext(DbContextOptions<FriendContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FriendMapping());
            modelBuilder.ApplyConfiguration(new FriendShipMapping());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<ApiFriends.Models.FriendShip> FriendShip { get; set; }

    }
}
