using FIAP.CloudGames.Application.Interfaces;
using FIAP.CloudGames.Application.UseCases.Authentication;
using FIAP.CloudGames.Domain.Enums;
using Microsoft.Extensions.Logging;
using FIAP.CloudGames.Domain.Entities;
using Moq;
using Xunit;

public class LoginUserUseCaseTests
{
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<ILogger<LoginUserUseCase>> _loggerMock;
    private readonly LoginUserUseCase _useCase;

    public LoginUserUseCaseTests()
    {
        _tokenServiceMock = new Mock<ITokenService>();
        _loggerMock = new Mock<ILogger<LoginUserUseCase>>();

        _tokenServiceMock
          .Setup(x => x.GenerateToken(It.IsAny<Users>()))
          .Returns("fake-jwt-token");

        _useCase = new LoginUserUseCase(
            _tokenServiceMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public void Execute_WhenAdminEmail_ShouldGenerateAdminToken()
    {
        // Arrange
        var email = "admin@fiap.com";

        // Act
        var token = _useCase.Execute(email);

        // Assert
        Assert.Equal("fake-jwt-token", token);

        _tokenServiceMock.Verify(x =>
           x.GenerateToken(It.Is<Users>(
               u => u.Role == UserRole.Admin)),
           Times.Once);

    }

    [Fact]
    public void Execute_WhenUserEmail_ShouldGenerateUserToken()
    {
        // Arrange
        var email = "user@fiap.com";

        // Act
        var token = _useCase.Execute(email);

        // Assert
        Assert.Equal("fake-jwt-token", token);

        _tokenServiceMock.Verify(x =>
            x.GenerateToken(It.Is<Users>(
                u => u.Role == UserRole.User)),
            Times.Once);

    }

    [Fact]
    public void Execute_WhenEmailIsEmpty_ShouldThrowException()
    {
        // Arrange
        var email = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _useCase.Execute(email));
    }
}
