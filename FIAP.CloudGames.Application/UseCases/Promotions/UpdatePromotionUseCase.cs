using FIAP.CloudGames.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Promotions
{
    public class UpdatePromotionUseCase
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly ILogger<UpdatePromotionUseCase> _logger;

        public UpdatePromotionUseCase(
            IPromotionRepository promotionRepository,
            ILogger<UpdatePromotionUseCase> logger)
        {
            _promotionRepository = promotionRepository;
            _logger = logger;
        }

        public async Task<PromotionResult<PromotionDto>> ExecuteAsync(Guid id, UpdatePromotionInput input)
        {
            _logger.LogInformation(
                "[App][UpdatePromotionUseCase] Starting update for promotion {PromotionId}",
                id);

            var validation = ValidateInput(
                input.Name,
                input.DiscountPercentage,
                input.StartsAt,
                input.EndsAt);

            if (!validation.Success)
            {
                _logger.LogWarning(
                    "[App][UpdatePromotionUseCase] Validation failed for promotion {PromotionId}",
                    id);

                return PromotionResult<PromotionDto>.Fail(
                    validation.Error,
                    validation.Message ?? "Invalid data.");
            }

            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion is null)
            {
                _logger.LogWarning(
                    "[App][UpdatePromotionUseCase] Promotion {PromotionId} not found",
                    id);

                return PromotionResult<PromotionDto>.Fail(
                    PromotionError.NotFound,
                    "Promotion not found.");
            }

            promotion.Update(
                input.Name,
                input.DiscountPercentage,
                input.StartsAt,
                input.EndsAt,
                input.Description,
                input.IsActive);

            await _promotionRepository.UpdateAsync(promotion);
            await _promotionRepository.SaveChangesAsync();

            _logger.LogInformation(
                "[App][UpdatePromotionUseCase] Promotion {PromotionId} updated",
                id);

            return PromotionResult<PromotionDto>.Ok(ToDto(promotion));
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

        private static PromotionResult ValidateInput(
            string name,
            decimal discountPercentage,
            DateTime startsAt,
            DateTime endsAt)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return PromotionResult.Fail(PromotionError.Validation, "Name is required.");
            }

            if (discountPercentage <= 0 || discountPercentage > 100)
            {
                return PromotionResult.Fail(PromotionError.Validation, "Discount must be between 0 and 100.");
            }

            if (endsAt <= startsAt)
            {
                return PromotionResult.Fail(PromotionError.Validation, "EndsAt must be after StartsAt.");
            }

            return PromotionResult.Ok();
        }
    }
}
