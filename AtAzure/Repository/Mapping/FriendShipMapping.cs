using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApiFriends.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiFriends.Repository.Mapping
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
