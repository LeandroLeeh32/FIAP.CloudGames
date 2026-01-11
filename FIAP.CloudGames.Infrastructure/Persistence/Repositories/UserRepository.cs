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
            "[Infra][UserRepository] Persistindo novo usuário {UserId} no banco",user.Id);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation(
            "[Infra][UserRepository] Buscando usuário por Id {UserId}",id);

        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        _logger.LogInformation(
            "[Infra][UserRepository] Buscando usuário por Email {Email}", email);

        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        _logger.LogInformation("[Infra][UserRepository] Buscando todos os usuários");

        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _logger.LogInformation(
            "[Infra][UserRepository] Atualizando usuário {UserId}",
            user.Id);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _logger.LogWarning(
            "[Infra][UserRepository] Removendo usuário {UserId}",
            user.Id);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
