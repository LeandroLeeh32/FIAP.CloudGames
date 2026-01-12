using FIAP.CloudGames.Application.Repositories;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.Infrastructure.Persistence.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly AppDbContext _dbContext;

        public PromotionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Promotion>> GetAllAsync()
        {
            return await _dbContext.Promotions
                .Include(p => p.PromotionGames)
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Promotion?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Promotions
                .Include(p => p.PromotionGames)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task AddAsync(Promotion promotion)
        {
            _dbContext.Promotions.Add(promotion);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Promotion promotion)
        {
            _dbContext.Promotions.Update(promotion);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Promotion promotion)
        {
            _dbContext.Promotions.Remove(promotion);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsPromotionGameAsync(Guid promotionId, Guid gameId)
        {
            return _dbContext.PromotionGames
                .AnyAsync(pg => pg.PromotionId == promotionId && pg.GameId == gameId);
        }

        public Task AddPromotionGameAsync(PromotionGame promotionGame)
        {
            _dbContext.PromotionGames.Add(promotionGame);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
