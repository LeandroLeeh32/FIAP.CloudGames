using FIAP.CloudGames.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Promotions
{
    public class DeletePromotionUseCase
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly ILogger<DeletePromotionUseCase> _logger;

        public DeletePromotionUseCase(
            IPromotionRepository promotionRepository,
            ILogger<DeletePromotionUseCase> logger)
        {
            _promotionRepository = promotionRepository;
            _logger = logger;
        }

        public async Task<PromotionResult> ExecuteAsync(Guid id)
        {
            _logger.LogInformation(
                "[App][DeletePromotionUseCase] Starting delete for promotion {PromotionId}",
                id);

            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion is null)
            {
                _logger.LogWarning(
                    "[App][DeletePromotionUseCase] Promotion {PromotionId} not found",
                    id);

                return PromotionResult.Fail(PromotionError.NotFound, "Promotion not found.");
            }

            await _promotionRepository.DeleteAsync(promotion);
            await _promotionRepository.SaveChangesAsync();

            _logger.LogInformation(
                "[App][DeletePromotionUseCase] Promotion {PromotionId} deleted",
                id);

            return PromotionResult.Ok();
        }
    }
}
