using FIAP.CloudGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.CloudGames.Infrastructure.Persistence.Configurations
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.ToTable("Promotions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(2000);

            builder.Property(p => p.DiscountPercentage)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(p => p.StartsAt)
                .IsRequired();

            builder.Property(p => p.EndsAt)
                .IsRequired();

            builder.Property(p => p.IsActive)
                .IsRequired();

            builder.HasMany(p => p.PromotionGames)
                .WithOne(pg => pg.Promotion)
                .HasForeignKey(pg => pg.PromotionId);
        }
    }
}
