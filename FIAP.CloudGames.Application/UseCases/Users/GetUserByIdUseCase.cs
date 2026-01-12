using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Users;

public class GetUserByIdUseCase
{
    private readonly IUserRepository _repository;
    private readonly ILogger<GetUserByIdUseCase> _logger;

    public GetUserByIdUseCase(
        IUserRepository repository,
        ILogger<GetUserByIdUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<User?> ExecuteAsync(Guid id)
    {
        _logger.LogInformation(
            "[App][GetUserByIdUseCase] Fetching user by id {UserId}",
            id);

        var user = await _repository.GetByIdAsync(id);

        if (user is null)
        {
            _logger.LogWarning(
                "[App][GetUserByIdUseCase] User {UserId} not found",
                id);
        }

        return user;
    }
}
