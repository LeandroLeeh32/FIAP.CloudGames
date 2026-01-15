using FIAP.CloudGames.Application.Repositories;
using FIAP.CloudGames.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.PromotionGames
{
    public class AddGameToPromotionUseCase
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<AddGameToPromotionUseCase> _logger;

        public AddGameToPromotionUseCase(
            IPromotionRepository promotionRepository,
            IGameRepository gameRepository,
            ILogger<AddGameToPromotionUseCase> logger)
        {
            _promotionRepository = promotionRepository;
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<PromotionGameResult> ExecuteAsync(Guid promotionId, Guid gameId)
        {
            _logger.LogInformation(
                "[App][AddGameToPromotionUseCase] Adding game {GameId} to promotion {PromotionId}",
                gameId,
                promotionId);

            if (promotionId == Guid.Empty || gameId == Guid.Empty)
            {
                _logger.LogWarning(
                    "[App][AddGameToPromotionUseCase] Missing ids. PromotionId: {PromotionId}, GameId: {GameId}",
                    promotionId,
                    gameId);

                return PromotionGameResult.Fail(
                    PromotionGameError.Validation,
                    "PromotionId and GameId are required.");
            }

            var promotion = await _promotionRepository.GetByIdAsync(promotionId);
            if (promotion is null)
            {
                _logger.LogWarning(
                    "[App][AddGameToPromotionUseCase] Promotion {PromotionId} not found",
                    promotionId);

                return PromotionGameResult.Fail(
                    PromotionGameError.NotFound,
                    "Promotion not found.");
            }

            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game is null)
            {
                _logger.LogWarning(
                    "[App][AddGameToPromotionUseCase] Game {GameId} not found",
                    gameId);

                return PromotionGameResult.Fail(
                    PromotionGameError.NotFound,
                    "Game not found.");
            }

            var exists = await _promotionRepository.ExistsPromotionGameAsync(promotionId, gameId);
            if (exists)
            {
                _logger.LogWarning(
                    "[App][AddGameToPromotionUseCase] Promotion {PromotionId} already has game {GameId}",
                    promotionId,
                    gameId);

                return PromotionGameResult.Fail(
                    PromotionGameError.Conflict,
                    "Promotion already applied to this game.");
            }

            await _promotionRepository.AddPromotionGameAsync(new PromotionGame(promotion, game));
            await _promotionRepository.SaveChangesAsync();

            _logger.LogInformation(
                "[App][AddGameToPromotionUseCase] Game {GameId} added to promotion {PromotionId}",
                gameId,
                promotionId);

            return PromotionGameResult.Ok();
        }
    }
}
