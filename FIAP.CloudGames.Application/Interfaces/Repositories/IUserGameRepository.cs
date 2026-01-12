using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Application.Interfaces.Repositories
{
    public interface IUserGameRepository
    {
        Task<bool> ExistsAsync(Guid userId, Guid gameId);
        Task AddAsync(UserGame userGame);
        Task SaveChangesAsync();
    }
}
