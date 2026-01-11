using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Users;
public class UpdateUserUseCase
{
    private readonly IUserRepository _repository;
    private readonly ILogger<UpdateUserUseCase> _logger;

    public UpdateUserUseCase( IUserRepository repository,ILogger<UpdateUserUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task ExecuteAsync(Guid id,string name,string email,UserRole role)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user is null)
        {
            _logger.LogWarning("[App][UpdateUserUseCase] Usuário {UserId} não encontrado", id);
            throw new Exception("[App][UpdateUserUseCase] Usuário não encontrado");
        }

        user.Update(name, email, role);

        await _repository.UpdateAsync(user);

        _logger.LogInformation("[App][UpdateUserUseCase] Usuário {UserId} atualizado com sucesso", id);
    }
}
