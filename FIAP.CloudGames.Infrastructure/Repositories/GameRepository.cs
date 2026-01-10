using FIAP.CloudGames.Application.Repositories;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GameRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Game>> GetAllAsync()
        {
            return await _dbContext.Games
                .AsNoTracking()
                .OrderBy(g => g.Title)
                .ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Games.FindAsync(id);
        }

        public Task AddAsync(Game game)
        {
            _dbContext.Games.Add(game);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Game game)
        {
            _dbContext.Games.Update(game);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Game game)
        {
            _dbContext.Games.Remove(game);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
