using FIAP.CloudGames.Application.Repositories;
using FIAP.CloudGames.Application.UseCases.Games;
using FIAP.CloudGames.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FIAP.CloudGames.Tests.Application.UseCases.Games;

public class GamesUseCasesTests
{
    [Fact]
    public async Task ExecuteAsync_CreateGame_ReturnsCreatedGame()
    {
        var input = new CreateGameInput
        {
            Title = "Game Title",
            Description = "Game Description",
            Price = 10m
        };

        var repository = new Mock<IGameRepository>();
        repository.Setup(r => r.AddAsync(It.IsAny<Game>())).Returns(Task.CompletedTask);
        repository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var sut = new CreateGameUseCase(
            repository.Object,
            new Mock<ILogger<CreateGameUseCase>>().Object);

        var result = await sut.ExecuteAsync(input);

        Assert.True(result.Success);
        Assert.Equal(input.Title, result.Data?.Title);
        Assert.Equal(input.Description, result.Data?.Description);
        Assert.Equal(input.Price, result.Data?.Price);
        repository.Verify(r => r.AddAsync(It.IsAny<Game>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_UpdateGame_ReturnsUpdatedGame()
    {
        var game = new Game("Old Title", 5m, "Old Description");
        var input = new UpdateGameInput
        {
            Title = "New Title",
            Description = "New Description",
            Price = 15m
        };

        var repository = new Mock<IGameRepository>();
        repository.Setup(r => r.GetByIdAsync(game.Id)).ReturnsAsync(game);
        repository.Setup(r => r.UpdateAsync(game)).Returns(Task.CompletedTask);
        repository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var sut = new UpdateGameUseCase(
            repository.Object,
            new Mock<ILogger<UpdateGameUseCase>>().Object);

        var result = await sut.ExecuteAsync(game.Id, input);

        Assert.True(result.Success);
        Assert.Equal(input.Title, result.Data?.Title);
        Assert.Equal(input.Description, result.Data?.Description);
        Assert.Equal(input.Price, result.Data?.Price);
        repository.Verify(r => r.UpdateAsync(game), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeleteGame_RemovesGame()
    {
        var game = new Game("Game Title", 10m, "Game Description");

        var repository = new Mock<IGameRepository>();
        repository.Setup(r => r.GetByIdAsync(game.Id)).ReturnsAsync(game);
        repository.Setup(r => r.DeleteAsync(game)).Returns(Task.CompletedTask);
        repository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var sut = new DeleteGameUseCase(
            repository.Object,
            new Mock<ILogger<DeleteGameUseCase>>().Object);

        var result = await sut.ExecuteAsync(game.Id);

        Assert.True(result.Success);
        repository.Verify(r => r.DeleteAsync(game), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_GetGameById_ReturnsGame()
    {
        var game = new Game("Game Title", 10m, "Game Description");

        var repository = new Mock<IGameRepository>();
        repository.Setup(r => r.GetByIdAsync(game.Id)).ReturnsAsync(game);

        var sut = new GetGameByIdUseCase(
            repository.Object,
            new Mock<ILogger<GetGameByIdUseCase>>().Object);

        var result = await sut.ExecuteAsync(game.Id);

        Assert.NotNull(result);
        Assert.Equal(game.Id, result?.Id);
    }

    [Fact]
    public async Task ExecuteAsync_GetGames_ReturnsList()
    {
        var games = new List<Game>
        {
            new Game("Game One", 10m, "One"),
            new Game("Game Two", 20m, "Two")
        };

        var repository = new Mock<IGameRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(games);

        var sut = new GetGamesUseCase(
            repository.Object,
            new Mock<ILogger<GetGamesUseCase>>().Object);

        var result = await sut.ExecuteAsync();

        Assert.Equal(2, result.Count);
    }
}
