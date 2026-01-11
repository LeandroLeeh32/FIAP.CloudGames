using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using Microsoft.Extensions.Logging;
using FIAP.CloudGames.Application.Interfaces.Security;

namespace FIAP.CloudGames.Application.UseCases.Users;

public class CreateUserUseCase
{
    private readonly IUserRepository _repository;
    private readonly ILogger<CreateUserUseCase> _logger;
    private readonly IPasswordHashService _passwordHashService;

    public CreateUserUseCase(
        IUserRepository repository,
        IPasswordHashService passwordHashService,
        ILogger<CreateUserUseCase> logger)
    {
        _repository = repository;
        _passwordHashService = passwordHashService;
        _logger = logger;
    }

    public async Task<Guid> ExecuteAsync(string name,string email, string password, UserRole role)
    {
        _logger.LogInformation(
            "[App][CreateUserUseCase] Iniciando criação de usuário. Email: {Email}",
            email);

        var existUser = await _repository.GetByEmailAsync(email);

        //if (existUser is not null)
        //{
        //    _logger.LogWarning(
        //        "[App][CreateUserUseCase] Usuário já existente com email {Email}",
        //        email);

        //    throw new Exception("Usuário já existente");
        //}

        var passwordHash = _passwordHashService.Hash(password);

        var user = User.Create(name, email, passwordHash, role);

        await _repository.AddAsync(user);

        _logger.LogInformation(
            "[App][CreateUserUseCase] Usuário criado com sucesso. Id: {UserId}",
            user.Id);

        return user.Id;
    }
}
