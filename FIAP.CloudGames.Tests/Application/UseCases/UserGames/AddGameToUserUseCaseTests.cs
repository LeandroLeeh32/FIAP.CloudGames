using FIAP.CloudGames.Application.Interfaces.Repositories;
using FIAP.CloudGames.Application.Repositories;
using FIAP.CloudGames.Application.UseCases.UserGames;
using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Domain.Enums;
using Moq;
using Xunit;

namespace FIAP.CloudGames.Tests.Application.UseCases.UserGames;

public class AddGameToUserUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ValidInput_AddsGameToUser()
    {
        var user = User.Create("User", "user@example.com", "hash", UserRole.User);
        var game = new Game("Game Title", 10m, "Game Description");

        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var gameRepository = new Mock<IGameRepository>();
        gameRepository.Setup(r => r.GetByIdAsync(game.Id)).ReturnsAsync(game);

        var userGameRepository = new Mock<IUserGameRepository>();
        userGameRepository.Setup(r => r.ExistsAsync(user.Id, game.Id)).ReturnsAsync(false);
        userGameRepository.Setup(r => r.AddAsync(It.IsAny<UserGame>())).Returns(Task.CompletedTask);
        userGameRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var sut = new AddGameToUserUseCase(
            userRepository.Object,
            userGameRepository.Object,
            gameRepository.Object);

        var result = await sut.ExecuteAsync(user.Id, game.Id);

        Assert.True(result.Success);
        userGameRepository.Verify(r => r.AddAsync(
            It.Is<UserGame>(ug => ug.UserId == user.Id && ug.GameId == game.Id)), Times.Once);
    }
}
