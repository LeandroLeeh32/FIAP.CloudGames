using FIAP.CloudGames.Application.Repositories;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Infrastructure.Persistence.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<GameRepository> _logger;

        public GameRepository(
            AppDbContext dbContext,
            ILogger<GameRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IReadOnlyList<Game>> GetAllAsync()
        {
            _logger.LogInformation("[Infra][GameRepository] Fetching all games");

            return await _dbContext.Games
                .AsNoTracking()
                .OrderBy(g => g.Title)
                .ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation(
                "[Infra][GameRepository] Fetching game by id {GameId}",
                id);

            return await _dbContext.Games.FindAsync(id);
        }

        public Task AddAsync(Game game)
        {
            _logger.LogInformation(
                "[Infra][GameRepository] Adding game {GameId}",
                game.Id);

            _dbContext.Games.Add(game);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Game game)
        {
            _logger.LogInformation(
                "[Infra][GameRepository] Updating game {GameId}",
                game.Id);

            _dbContext.Games.Update(game);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Game game)
        {
            _logger.LogWarning(
                "[Infra][GameRepository] Deleting game {GameId}",
                game.Id);

            _dbContext.Games.Remove(game);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            _logger.LogInformation("[Infra][GameRepository] Saving changes");

            return _dbContext.SaveChangesAsync();
        }
    }
}
