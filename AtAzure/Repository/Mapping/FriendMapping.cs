using ApiFriends.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiFriends.Repository.Mapping
{
    public class FriendMapping : IEntityTypeConfiguration<Friend>
    {
        public void Configure(EntityTypeBuilder<Friend> builder)
        {
            builder.ToTable("Friend");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.LastName).IsRequired();
            builder.Property(x => x.PhotoUrl).IsRequired();
            builder.Property(x => x.Phone).IsRequired();
            builder.Property(x => x.CountryId).IsRequired();
            builder.Property(x => x.StateId).IsRequired();

            builder.HasOne(x => x.State);
            builder.HasOne(x => x.Country);

            builder.HasMany(x => x.FriendShips).WithOne(x => x.UserOrFriend).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.FriendShips).WithOne(x => x.UserOrFriend).HasForeignKey(x => x.FriendId);
        }
    }
}
