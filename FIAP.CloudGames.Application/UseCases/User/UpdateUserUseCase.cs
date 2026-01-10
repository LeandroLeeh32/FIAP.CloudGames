using FIAP.CloudGames.Application.Interfaces;
using FIAP.CloudGames.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.User;
public class UpdateUserUseCase
{
    private readonly IUserRepository _repository;
    private readonly ILogger<UpdateUserUseCase> _logger;

    public UpdateUserUseCase( IUserRepository repository,ILogger<UpdateUserUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public void Execute(Guid id, string name, string email, UserRole role)
    {
        _logger.LogInformation(
            "[App][UpdateUserUseCase] Iniciando atualização do usuário {UserId}",
            id);

        var user = _repository.GetById(id);

        if (user == null)
        {
            _logger.LogWarning(
                "[App][UpdateUserUseCase] Usuário não encontrado: {UserId}",
                id);

            throw new Exception("Usuário não encontrado");
        }

        user.Update(name, email, role);
        _repository.Update(user);

        _logger.LogInformation(
            "[App][UpdateUserUseCase] Usuário atualizado com sucesso: {UserId}",
            id);
    }
}
