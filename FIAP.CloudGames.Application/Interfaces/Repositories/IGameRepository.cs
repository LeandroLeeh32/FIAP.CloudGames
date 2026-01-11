using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Application.Repositories
{
    public interface IGameRepository
    {
        Task<IReadOnlyList<Game>> GetAllAsync();
        Task<Game?> GetByIdAsync(Guid id);
        Task AddAsync(Game game);
        Task UpdateAsync(Game game);
        Task DeleteAsync(Game game);
        Task SaveChangesAsync();
    }
}
