using FIAP.CloudGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.CloudGames.Infrastructure.Persistence.Configurations
{
    public class UserGameConfiguration : IEntityTypeConfiguration<UserGame>
    {
        public void Configure(EntityTypeBuilder<UserGame> builder)
        {
            builder.ToTable("UserGames");

            builder.HasKey(ug => new { ug.UserId, ug.GameId });

            builder.Property(ug => ug.PurchasedAt)
                .IsRequired();
        }
    }
}
