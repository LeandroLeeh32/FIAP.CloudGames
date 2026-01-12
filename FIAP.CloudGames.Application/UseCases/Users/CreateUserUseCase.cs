using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Application.Interfaces.Security;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using Microsoft.Extensions.Logging;

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

    public async Task<UserResult<Guid>> ExecuteAsync(string name, string email, string password, UserRole role)
    {
        _logger.LogInformation(
            "[App][CreateUserUseCase] Starting user creation. Email: {Email}",
            email);

        var validation = ValidateInput(name, email, password);
        if (!validation.Success)
        {
            _logger.LogWarning(
                "[App][CreateUserUseCase] Validation failed for email {Email}",
                email);

            return UserResult<Guid>.Fail(
                validation.Error,
                validation.Message ?? "Invalid data.");
        }

        var existingUser = await _repository.GetByEmailAsync(email);
        if (existingUser is not null)
        {
            _logger.LogWarning(
                "[App][CreateUserUseCase] User already exists for email {Email}",
                email);

            return UserResult<Guid>.Fail(
                UserError.Conflict,
                "User already exists.");
        }

        var passwordHash = _passwordHashService.Hash(password);
        var user = User.Create(name, email, passwordHash, role);

        await _repository.AddAsync(user);

        _logger.LogInformation(
            "[App][CreateUserUseCase] User created successfully. Id: {UserId}",
            user.Id);

        return UserResult<Guid>.Ok(user.Id);
    }

    private static UserResult ValidateInput(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return UserResult.Fail(UserError.Validation, "Name is required.");
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
        {
            return UserResult.Fail(UserError.Validation, "Email is invalid.");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            return UserResult.Fail(UserError.Validation, "Password is required.");
        }

        return UserResult.Ok();
    }
}
