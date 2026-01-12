using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Users;

public class UpdateUserUseCase
{
    private readonly IUserRepository _repository;
    private readonly ILogger<UpdateUserUseCase> _logger;

    public UpdateUserUseCase(IUserRepository repository, ILogger<UpdateUserUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<UserResult> ExecuteAsync(Guid id, string name, string email, UserRole role)
    {
        var validation = ValidateInput(name, email);
        if (!validation.Success)
        {
            return validation;
        }

        var user = await _repository.GetByIdAsync(id);

        if (user is null)
        {
            _logger.LogWarning(
                "[App][UpdateUserUseCase] User {UserId} not found",
                id);

            return UserResult.Fail(UserError.NotFound, "User not found.");
        }

        user.Update(name, email, role);

        await _repository.UpdateAsync(user);

        _logger.LogInformation(
            "[App][UpdateUserUseCase] User {UserId} updated successfully",
            id);

        return UserResult.Ok();
    }

    private static UserResult ValidateInput(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return UserResult.Fail(UserError.Validation, "Name is required.");
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
        {
            return UserResult.Fail(UserError.Validation, "Email is invalid.");
        }

        return UserResult.Ok();
    }
}
