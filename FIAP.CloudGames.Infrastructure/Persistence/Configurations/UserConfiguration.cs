using FIAP.CloudGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.CloudGames.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.PasswordHash)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasMany(u => u.UserGames)
                .WithOne(ug => ug.User)
                .HasForeignKey(ug => ug.UserId);
        }
    }
}
