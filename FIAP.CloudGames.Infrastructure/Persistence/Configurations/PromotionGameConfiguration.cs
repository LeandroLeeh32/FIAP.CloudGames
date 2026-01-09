using FIAP.CloudGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.CloudGames.Infrastructure.Persistence.Configurations
{
    public class PromotionGameConfiguration : IEntityTypeConfiguration<PromotionGame>
    {
        public void Configure(EntityTypeBuilder<PromotionGame> builder)
        {
            builder.ToTable("PromotionGames");

            builder.HasKey(pg => new { pg.PromotionId, pg.GameId });

            builder.HasOne(pg => pg.Promotion)
                .WithMany(p => p.PromotionGames)
                .HasForeignKey(pg => pg.PromotionId);

            builder.HasOne(pg => pg.Game)
                .WithMany(g => g.PromotionGames)
                .HasForeignKey(pg => pg.GameId);
        }
    }
}
