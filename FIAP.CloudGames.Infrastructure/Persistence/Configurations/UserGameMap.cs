using FIAP.CloudGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.CloudGames.Infrastructure.Persistence.Configurations
{
    public class UserGameMap : IEntityTypeConfiguration<UserGame>
    {
        public void Configure(EntityTypeBuilder<UserGame> builder)
        {
            builder.ToTable("UserGames");

            builder.HasKey(x => new { x.UserId, x.GameId });

            builder.Property(x => x.PurchasedAt)
                   .IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany(x => x.UserGames)
                   .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Game)
                   .WithMany(x => x.UserGames)
                   .HasForeignKey(x => x.GameId);
        }
    }
}
