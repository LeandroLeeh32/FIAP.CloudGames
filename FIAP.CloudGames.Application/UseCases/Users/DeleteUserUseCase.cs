using FIAP.CloudGames.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Users;

public class DeleteUserUseCase
{
    private readonly IUserRepository _repository;
    private readonly ILogger<DeleteUserUseCase> _logger;

    public DeleteUserUseCase(
        IUserRepository repository,
        ILogger<DeleteUserUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<UserResult> ExecuteAsync(Guid id)
    {
        _logger.LogInformation(
            "[App][DeleteUserUseCase] Starting delete for user {UserId}",
            id);

        var user = await _repository.GetByIdAsync(id);

        if (user is null)
        {
            _logger.LogWarning(
                "[App][DeleteUserUseCase] User {UserId} not found",
                id);

            return UserResult.Fail(UserError.NotFound, "User not found.");
        }

        await _repository.DeleteAsync(user);

        _logger.LogInformation(
            "[App][DeleteUserUseCase] User {UserId} removed",
            id);

        return UserResult.Ok();
    }
}
