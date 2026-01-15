using FIAP.CloudGames.Application.Repositories;
using FIAP.CloudGames.Application.UseCases.PromotionGames;
using FIAP.CloudGames.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FIAP.CloudGames.Tests.Application.UseCases.PromotionGames;

public class AddGameToPromotionUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ValidInput_AddsGameToPromotion()
    {
        var startsAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endsAt = startsAt.AddDays(10);
        var promotion = new Promotion("Promo", 10m, startsAt, endsAt, "Promo Description");
        var game = new Game("Game Title", 10m, "Game Description");

        var promotionRepository = new Mock<IPromotionRepository>();
        promotionRepository.Setup(r => r.GetByIdAsync(promotion.Id)).ReturnsAsync(promotion);
        promotionRepository.Setup(r => r.ExistsPromotionGameAsync(promotion.Id, game.Id))
            .ReturnsAsync(false);
        promotionRepository.Setup(r => r.AddPromotionGameAsync(It.IsAny<PromotionGame>()))
            .Returns(Task.CompletedTask);
        promotionRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var gameRepository = new Mock<IGameRepository>();
        gameRepository.Setup(r => r.GetByIdAsync(game.Id)).ReturnsAsync(game);

        var sut = new AddGameToPromotionUseCase(
            promotionRepository.Object,
            gameRepository.Object,
            new Mock<ILogger<AddGameToPromotionUseCase>>().Object);

        var result = await sut.ExecuteAsync(promotion.Id, game.Id);

        Assert.True(result.Success);
        promotionRepository.Verify(r => r.AddPromotionGameAsync(
            It.Is<PromotionGame>(pg => pg.PromotionId == promotion.Id && pg.GameId == game.Id)), Times.Once);
    }
}
