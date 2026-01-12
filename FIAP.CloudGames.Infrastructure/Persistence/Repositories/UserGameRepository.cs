using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FIAP.CloudGames.Infrastructure.Persistence.Repositories
{
    public class UserGameRepository : IUserGameRepository
    {
        private readonly AppDbContext _dbContext;

        public UserGameRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> ExistsAsync(Guid userId, Guid gameId)
        {
            return _dbContext.UserGames
                .AnyAsync(ug => ug.UserId == userId && ug.GameId == gameId);
        }

        public Task AddAsync(UserGame userGame)
        {
            _dbContext.UserGames.Add(userGame);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
