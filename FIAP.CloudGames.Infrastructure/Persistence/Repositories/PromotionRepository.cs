using FIAP.CloudGames.Application.Repositories;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Infrastructure.Persistence.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PromotionRepository> _logger;

        public PromotionRepository(
            AppDbContext dbContext,
            ILogger<PromotionRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IReadOnlyList<Promotion>> GetAllAsync()
        {
            _logger.LogInformation("[Infra][PromotionRepository] Fetching all promotions");

            return await _dbContext.Promotions
                .Include(p => p.PromotionGames)
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Promotion?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation(
                "[Infra][PromotionRepository] Fetching promotion by id {PromotionId}",
                id);

            return await _dbContext.Promotions
                .Include(p => p.PromotionGames)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task AddAsync(Promotion promotion)
        {
            _logger.LogInformation(
                "[Infra][PromotionRepository] Adding promotion {PromotionId}",
                promotion.Id);

            _dbContext.Promotions.Add(promotion);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Promotion promotion)
        {
            _logger.LogInformation(
                "[Infra][PromotionRepository] Updating promotion {PromotionId}",
                promotion.Id);

            _dbContext.Promotions.Update(promotion);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Promotion promotion)
        {
            _logger.LogWarning(
                "[Infra][PromotionRepository] Deleting promotion {PromotionId}",
                promotion.Id);

            _dbContext.Promotions.Remove(promotion);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsPromotionGameAsync(Guid promotionId, Guid gameId)
        {
            _logger.LogInformation(
                "[Infra][PromotionRepository] Checking if promotion {PromotionId} has game {GameId}",
                promotionId,
                gameId);

            return _dbContext.PromotionGames
                .AnyAsync(pg => pg.PromotionId == promotionId && pg.GameId == gameId);
        }

        public Task AddPromotionGameAsync(PromotionGame promotionGame)
        {
            _logger.LogInformation(
                "[Infra][PromotionRepository] Adding game {GameId} to promotion {PromotionId}",
                promotionGame.GameId,
                promotionGame.PromotionId);

            _dbContext.PromotionGames.Add(promotionGame);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            _logger.LogInformation("[Infra][PromotionRepository] Saving changes");

            return _dbContext.SaveChangesAsync();
        }
    }
}
