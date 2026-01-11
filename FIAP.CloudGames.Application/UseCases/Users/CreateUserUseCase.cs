using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Users;

public class CreateUserUseCase
{
    private readonly IUserRepository _repository;
    private readonly ILogger<CreateUserUseCase> _logger;

    public CreateUserUseCase(
        IUserRepository repository,
        ILogger<CreateUserUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Guid> ExecuteAsync(string name,string email,UserRole role)
    {
        _logger.LogInformation(
            "[App][CreateUserUseCase] Iniciando criação de usuário. Email: {Email}",
            email);

        var existingUser = await _repository.GetByEmailAsync(email);

        if (existingUser is not null)
        {
            _logger.LogWarning(
                "[App][CreateUserUseCase] Usuário já existe com email {Email}",
                email);

            throw new Exception("Usuário já existe");
        }

        var user = User.Create(name, email, role);

        await _repository.AddAsync(user);

        _logger.LogInformation(
            "[App][CreateUserUseCase] Usuário criado com sucesso. Id: {UserId}",
            user.Id);

        return user.Id;
    }
}
