using FIAP.CloudGames.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Promotions
{
    public class GetPromotionsUseCase
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly ILogger<GetPromotionsUseCase> _logger;

        public GetPromotionsUseCase(
            IPromotionRepository promotionRepository,
            ILogger<GetPromotionsUseCase> logger)
        {
            _promotionRepository = promotionRepository;
            _logger = logger;
        }

        public async Task<IReadOnlyList<PromotionDto>> ExecuteAsync()
        {
            _logger.LogInformation("[App][GetPromotionsUseCase] Fetching promotions list");

            var promotions = await _promotionRepository.GetAllAsync();
            var result = promotions.Select(ToDto).ToList();

            _logger.LogInformation(
                "[App][GetPromotionsUseCase] Promotions fetched. Total: {Total}",
                result.Count);

            return result;
        }

        private static PromotionDto ToDto(Domain.Entities.Promotion promotion)
        {
            return new PromotionDto
            {
                Id = promotion.Id,
                Name = promotion.Name,
                Description = promotion.Description,
                DiscountPercentage = promotion.DiscountPercentage,
                StartsAt = promotion.StartsAt,
                EndsAt = promotion.EndsAt,
                IsActive = promotion.IsActive,
                GameIds = promotion.PromotionGames.Select(pg => pg.GameId).ToList()
            };
        }
    }
}
