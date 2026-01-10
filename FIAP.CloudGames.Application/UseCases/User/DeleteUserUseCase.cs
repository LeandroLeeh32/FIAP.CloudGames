using FIAP.CloudGames.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.User;
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

    public void Execute(Guid id)
    {
        _repository.Delete(id);

        _logger.LogInformation(
            "[App][DeleteUserUseCase] Usuário removido: {UserId}",
            id);
    }
}
