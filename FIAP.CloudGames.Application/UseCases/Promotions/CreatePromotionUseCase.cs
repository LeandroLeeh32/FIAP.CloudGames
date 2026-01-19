using FIAP.CloudGames.Application.Repositories;
using FIAP.CloudGames.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Promotions
{
    public class CreatePromotionUseCase
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly ILogger<CreatePromotionUseCase> _logger;

        public CreatePromotionUseCase(
            IPromotionRepository promotionRepository,
            ILogger<CreatePromotionUseCase> logger)
        {
            _promotionRepository = promotionRepository;
            _logger = logger;
        }

        public async Task<PromotionResult<PromotionDto>> ExecuteAsync(CreatePromotionInput input)
        {
            _logger.LogInformation(
                "[App][CreatePromotionUseCase] Starting create for promotion {Name}",
                input.Name);

            var validation = ValidateInput(
                input.Name,
                input.DiscountPercentage,
                input.StartsAt,
                input.EndsAt);

            if (!validation.Success)
            {
                _logger.LogWarning(
                    "[App][CreatePromotionUseCase] Validation failed for promotion {Name}",
                    input.Name);

                return PromotionResult<PromotionDto>.Fail(
                    validation.Error,
                    validation.Message ?? "Invalid data.");
            }

            var promotion = new Promotion(
                input.Name,
                input.DiscountPercentage,
                input.StartsAt,
                input.EndsAt,
                input.Description);

            await _promotionRepository.AddAsync(promotion);
            await _promotionRepository.SaveChangesAsync();

            _logger.LogInformation(
                "[App][CreatePromotionUseCase] Promotion created with id {PromotionId}",
                promotion.Id);

            return PromotionResult<PromotionDto>.Ok(ToDto(promotion));
        }

        private static PromotionDto ToDto(Promotion promotion)
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
