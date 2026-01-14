using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Application.Interfaces.Security;
using FIAP.CloudGames.Application.UseCases.Users;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FIAP.CloudGames.Tests.Application.UseCases.Users;

public class CreateUserUseCaseTests
{
    private readonly Mock<IUserRepository> _repository = new();
    private readonly Mock<IPasswordHashService> _passwordHashService = new();
    private readonly Mock<ILogger<CreateUserUseCase>> _logger = new();

    private CreateUserUseCase CreateSut()
        => new(_repository.Object, _passwordHashService.Object, _logger.Object);

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("user@")]
    public async Task ExecuteAsync_InvalidEmail_ReturnsValidationError(string email)
    {
        var sut = CreateSut();

        var result = await sut.ExecuteAsync("User", email, "Abc123!@", UserRole.User);

        Assert.False(result.Success);
        Assert.Equal(UserError.Validation, result.Error);
        Assert.Equal("Email is invalid.", result.Message);
    }

    [Theory]
    [InlineData("1234@!89")]
    [InlineData("$$$12345")]
    public async Task ExecuteAsync_PasswordWithoutLetter_ReturnsValidationError(string password)
    {
        var sut = CreateSut();

        var result = await sut.ExecuteAsync("User", "user@example.com", password, UserRole.User);

        Assert.False(result.Success);
        Assert.Equal(UserError.Validation, result.Error);
        Assert.Equal(
            "Password must contain at least one letter, one number, and one special character.",
            result.Message);
    }

    [Theory]
    [InlineData("Abcdef!!")]
    [InlineData("ABC$$$$$")]
    public async Task ExecuteAsync_PasswordWithoutNumber_ReturnsValidationError(string password)
    {
        var sut = CreateSut();

        var result = await sut.ExecuteAsync("User", "user@example.com", password, UserRole.User);

        Assert.False(result.Success);
        Assert.Equal(UserError.Validation, result.Error);
        Assert.Equal(
            "Password must contain at least one letter, one number, and one special character.",
            result.Message);
    }

    [Theory]
    [InlineData("Abc12345")]
    [InlineData("Password1")]
    public async Task ExecuteAsync_PasswordWithoutSpecial_ReturnsValidationError(string password)
    {
        var sut = CreateSut();

        var result = await sut.ExecuteAsync("User", "user@example.com", password, UserRole.User);

        Assert.False(result.Success);
        Assert.Equal(UserError.Validation, result.Error);
        Assert.Equal(
            "Password must contain at least one letter, one number, and one special character.",
            result.Message);
    }

    [Theory]
    [InlineData("Ab1!c")]
    [InlineData("A1!bc2")]
    public async Task ExecuteAsync_PasswordTooShort_ReturnsValidationError(string password)
    {
        var sut = CreateSut();

        var result = await sut.ExecuteAsync("User", "user@example.com", password, UserRole.User);

        Assert.False(result.Success);
        Assert.Equal(UserError.Validation, result.Error);
        Assert.Equal("Password must be at least 8 characters.", result.Message);
    }

    [Fact]
    public async Task ExecuteAsync_ValidInput_CreatesUser()
    {
        const string name = "User";
        const string email = "user@example.com";
        const string password = "Abc123!@";

        _repository.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync((User?)null);
        _passwordHashService.Setup(h => h.Hash(password)).Returns("hashed");

        var sut = CreateSut();

        var result = await sut.ExecuteAsync(name, email, password, UserRole.User);

        Assert.True(result.Success);
        Assert.Equal(UserError.None, result.Error);
        Assert.NotEqual(Guid.Empty, result.Data);
        _repository.Verify(r => r.AddAsync(It.Is<User>(u =>
            u.Name == name &&
            u.Email == email &&
            u.PasswordHash == "hashed" &&
            u.Role == UserRole.User)), Times.Once);
    }
}
