using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task DeleteAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task UpdateAsync(User user);
        Task<bool> ExistsGameAsync(Guid userId, Guid gameId);
        Task AddGameAsync(UserGame userGame);
        Task SaveChangesAsync();
    }
}
