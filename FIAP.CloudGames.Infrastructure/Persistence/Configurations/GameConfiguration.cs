using FIAP.CloudGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.CloudGames.Infrastructure.Persistence.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("Games");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(g => g.Description)
                .HasMaxLength(2000);

            builder.Property(g => g.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasMany(g => g.UserGames)
                .WithOne(ug => ug.Game)
                .HasForeignKey(ug => ug.GameId);

            builder.HasMany(g => g.PromotionGames)
                .WithOne(pg => pg.Game)
                .HasForeignKey(pg => pg.GameId);
        }
    }
}
