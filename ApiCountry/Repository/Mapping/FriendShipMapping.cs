
using ApiCountry.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCountry.Repository.Mapping
{
    public class FriendShipMapping : IEntityTypeConfiguration<FriendShip>
    {
        public void Configure(EntityTypeBuilder<FriendShip> builder)
        {
            builder.ToTable("FriendShip");
            builder.HasKey(x => new { x.UserId, x.FriendId });
            builder.HasOne(x => x.UserOrFriend).WithMany(x => x.FriendShips).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.UserOrFriend).WithMany(x => x.FriendShips).HasForeignKey(x => x.FriendId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
