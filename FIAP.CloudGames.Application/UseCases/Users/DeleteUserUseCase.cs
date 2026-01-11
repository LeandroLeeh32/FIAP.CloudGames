using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Entities;
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

    public async Task ExecuteAsync(Guid id)
    {
        _logger.LogInformation("[App][DeleteUserUseCase] Iniciando exclusão do usuário {UserId}", id);

        var user = await _repository.GetByIdAsync(id);

        if (user is null)
        {
            _logger.LogWarning("[App][DeleteUserUseCase] Usuário {UserId} não encontrado para exclusão", id);
            throw new Exception("Usuário não encontrado");
        }

        await _repository.DeleteAsync(user);
        _logger.LogInformation("[App][DeleteUserUseCase] Usuário {UserId} removido com sucesso", id);
    }
}
