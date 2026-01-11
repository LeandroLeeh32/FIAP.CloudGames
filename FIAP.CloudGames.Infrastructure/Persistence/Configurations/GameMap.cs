using FIAP.CloudGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GameMap : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(x => x.Price)
               .IsRequired();
    }
}
