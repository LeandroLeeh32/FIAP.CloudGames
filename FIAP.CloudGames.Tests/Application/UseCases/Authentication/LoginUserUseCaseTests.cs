using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Application.Interfaces.Security;
using FIAP.CloudGames.Application.Interfaces.Services;
using FIAP.CloudGames.Application.UseCases.Authentication;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FIAP.CloudGames.Tests.Application.UseCases.Authentication;

public class LoginUserUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ValidCredentials_ReturnsToken()
    {
        const string email = "user@example.com";
        const string password = "Secret123!";
        const string token = "token";
        var user = User.Create("User", email, "hashed", UserRole.User);

        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);

        var hashService = new Mock<IPasswordHashService>();
        hashService.Setup(h => h.Verify(password, user.PasswordHash)).Returns(true);

        var tokenService = new Mock<ITokenService>();
        tokenService.Setup(t => t.GenerateToken(user)).Returns(token);

        var logger = new Mock<ILogger<LoginUserUseCase>>();
        var sut = new LoginUserUseCase(
            repository.Object,
            hashService.Object,
            tokenService.Object,
            logger.Object);

        var result = await sut.ExecuteAsync(email, password);

        Assert.Equal(token, result);
        tokenService.Verify(t => t.GenerateToken(user), Times.Once);
    }
}
