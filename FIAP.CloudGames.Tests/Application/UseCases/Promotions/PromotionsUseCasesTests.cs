using FIAP.CloudGames.Application.Repositories;
using FIAP.CloudGames.Application.UseCases.Promotions;
using FIAP.CloudGames.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FIAP.CloudGames.Tests.Application.UseCases.Promotions;

public class PromotionsUseCasesTests
{
    [Fact]
    public async Task ExecuteAsync_CreatePromotion_ReturnsCreatedPromotion()
    {
        var startsAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endsAt = startsAt.AddDays(10);
        var input = new CreatePromotionInput
        {
            Name = "Promo",
            Description = "Promo Description",
            DiscountPercentage = 10m,
            StartsAt = startsAt,
            EndsAt = endsAt
        };

        var repository = new Mock<IPromotionRepository>();
        repository.Setup(r => r.AddAsync(It.IsAny<Promotion>())).Returns(Task.CompletedTask);
        repository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var sut = new CreatePromotionUseCase(
            repository.Object,
            new Mock<ILogger<CreatePromotionUseCase>>().Object);

        var result = await sut.ExecuteAsync(input);

        Assert.True(result.Success);
        Assert.Equal(input.Name, result.Data?.Name);
        Assert.Equal(input.DiscountPercentage, result.Data?.DiscountPercentage);
        repository.Verify(r => r.AddAsync(It.IsAny<Promotion>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_UpdatePromotion_ReturnsUpdatedPromotion()
    {
        var startsAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endsAt = startsAt.AddDays(10);
        var promotion = new Promotion("Promo", 10m, startsAt, endsAt, "Promo Description");

        var input = new UpdatePromotionInput
        {
            Name = "Updated Promo",
            Description = "Updated Description",
            DiscountPercentage = 20m,
            StartsAt = startsAt,
            EndsAt = endsAt.AddDays(1),
            IsActive = false
        };

        var repository = new Mock<IPromotionRepository>();
        repository.Setup(r => r.GetByIdAsync(promotion.Id)).ReturnsAsync(promotion);
        repository.Setup(r => r.UpdateAsync(promotion)).Returns(Task.CompletedTask);
        repository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var sut = new UpdatePromotionUseCase(
            repository.Object,
            new Mock<ILogger<UpdatePromotionUseCase>>().Object);

        var result = await sut.ExecuteAsync(promotion.Id, input);

        Assert.True(result.Success);
        Assert.Equal(input.Name, result.Data?.Name);
        Assert.Equal(input.DiscountPercentage, result.Data?.DiscountPercentage);
        repository.Verify(r => r.UpdateAsync(promotion), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeletePromotion_RemovesPromotion()
    {
        var startsAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endsAt = startsAt.AddDays(10);
        var promotion = new Promotion("Promo", 10m, startsAt, endsAt, "Promo Description");

        var repository = new Mock<IPromotionRepository>();
        repository.Setup(r => r.GetByIdAsync(promotion.Id)).ReturnsAsync(promotion);
        repository.Setup(r => r.DeleteAsync(promotion)).Returns(Task.CompletedTask);
        repository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var sut = new DeletePromotionUseCase(
            repository.Object,
            new Mock<ILogger<DeletePromotionUseCase>>().Object);

        var result = await sut.ExecuteAsync(promotion.Id);

        Assert.True(result.Success);
        repository.Verify(r => r.DeleteAsync(promotion), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_GetPromotionById_ReturnsPromotion()
    {
        var startsAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endsAt = startsAt.AddDays(10);
        var promotion = new Promotion("Promo", 10m, startsAt, endsAt, "Promo Description");

        var repository = new Mock<IPromotionRepository>();
        repository.Setup(r => r.GetByIdAsync(promotion.Id)).ReturnsAsync(promotion);

        var sut = new GetPromotionByIdUseCase(
            repository.Object,
            new Mock<ILogger<GetPromotionByIdUseCase>>().Object);

        var result = await sut.ExecuteAsync(promotion.Id);

        Assert.NotNull(result);
        Assert.Equal(promotion.Id, result?.Id);
    }

    [Fact]
    public async Task ExecuteAsync_GetPromotions_ReturnsList()
    {
        var startsAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endsAt = startsAt.AddDays(10);
        var promotions = new List<Promotion>
        {
            new Promotion("Promo One", 10m, startsAt, endsAt, "One"),
            new Promotion("Promo Two", 20m, startsAt, endsAt, "Two")
        };

        var repository = new Mock<IPromotionRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(promotions);

        var sut = new GetPromotionsUseCase(
            repository.Object,
            new Mock<ILogger<GetPromotionsUseCase>>().Object);

        var result = await sut.ExecuteAsync();

        Assert.Equal(2, result.Count);
    }

}
