using FIAP.CloudGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PromotionGameMap : IEntityTypeConfiguration<PromotionGame>
{
    public void Configure(EntityTypeBuilder<PromotionGame> builder)
    {
        builder.ToTable("PromotionGames");

        builder.HasKey(x => new { x.PromotionId, x.GameId });

        builder.HasOne(x => x.Promotion)
               .WithMany(x => x.PromotionGames)
               .HasForeignKey(x => x.PromotionId);

        builder.HasOne(x => x.Game)
               .WithMany(x => x.PromotionGames)
               .HasForeignKey(x => x.GameId);
    }
}
