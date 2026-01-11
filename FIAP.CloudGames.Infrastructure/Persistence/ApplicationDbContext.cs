using FIAP.CloudGames.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Game> Games => Set<Game>();
        public DbSet<UserGame> UserGames => Set<UserGame>();
        public DbSet<Promotion> Promotions => Set<Promotion>();
        public DbSet<PromotionGame> PromotionGames => Set<PromotionGame>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);
        }
    }
}
