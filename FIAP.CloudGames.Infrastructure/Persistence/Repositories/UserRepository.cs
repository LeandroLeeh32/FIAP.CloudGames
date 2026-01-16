using FIAP.CloudGames.Application.Interfaces;
using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        AppDbContext context,
        ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(User user)
    {
        _logger.LogInformation(
            "[Infra][UserRepository] Persisting new user {UserId} to database",
            user.Id);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation(
            "[Infra][UserRepository] Fetching user by id {UserId}",
            id);

        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        _logger.LogInformation(
            "[Infra][UserRepository] Fetching user by email {Email}",
            email);

        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        _logger.LogInformation("[Infra][UserRepository] Fetching all users");

        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _logger.LogInformation(
            "[Infra][UserRepository] Updating user {UserId}",
            user.Id);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _logger.LogWarning(
            "[Infra][UserRepository] Removing user {UserId}",
            user.Id);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsGameAsync(Guid userId, Guid gameId)
    {
        _logger.LogInformation(
            "[Infra][UserRepository] Checking if user {UserId} owns game {GameId}",
            userId,
            gameId);

        return _context.UserGames
            .AnyAsync(ug => ug.UserId == userId && ug.GameId == gameId);
    }

    public Task AddGameAsync(UserGame userGame)
    {
        _logger.LogInformation(
            "[Infra][UserRepository] Adding game {GameId} to user {UserId}",
            userGame.GameId,
            userGame.UserId);

        _context.UserGames.Add(userGame);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        _logger.LogInformation("[Infra][UserRepository] Saving changes");

        return _context.SaveChangesAsync();
    }
}
