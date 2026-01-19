using FIAP.CloudGames.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Promotions
{
    public class GetPromotionByIdUseCase
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly ILogger<GetPromotionByIdUseCase> _logger;

        public GetPromotionByIdUseCase(
            IPromotionRepository promotionRepository,
            ILogger<GetPromotionByIdUseCase> logger)
        {
            _promotionRepository = promotionRepository;
            _logger = logger;
        }

        public async Task<PromotionDto?> ExecuteAsync(Guid id)
        {
            _logger.LogInformation(
                "[App][GetPromotionByIdUseCase] Fetching promotion {PromotionId}",
                id);

            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion is null)
            {
                _logger.LogWarning(
                    "[App][GetPromotionByIdUseCase] Promotion {PromotionId} not found",
                    id);

                return null;
            }

            return ToDto(promotion);
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
