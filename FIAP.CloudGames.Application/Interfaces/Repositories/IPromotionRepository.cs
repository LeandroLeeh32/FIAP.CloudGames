using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Application.Repositories
{
    public interface IPromotionRepository
    {
        Task<IReadOnlyList<Promotion>> GetAllAsync();
        Task<Promotion?> GetByIdAsync(Guid id);
        Task AddAsync(Promotion promotion);
        Task UpdateAsync(Promotion promotion);
        Task DeleteAsync(Promotion promotion);
        Task<bool> ExistsPromotionGameAsync(Guid promotionId, Guid gameId);
        Task AddPromotionGameAsync(PromotionGame promotionGame);
        Task SaveChangesAsync();
    }
}
