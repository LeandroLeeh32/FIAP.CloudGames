using FIAP.CloudGames.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class DeleteGameUseCase
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<DeleteGameUseCase> _logger;

        public DeleteGameUseCase(
            IGameRepository gameRepository,
            ILogger<DeleteGameUseCase> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<GameResult> ExecuteAsync(Guid id)
        {
            _logger.LogInformation(
                "[App][DeleteGameUseCase] Starting delete for game {GameId}",
                id);

            var game = await _gameRepository.GetByIdAsync(id);
            if (game is null)
            {
                _logger.LogWarning(
                    "[App][DeleteGameUseCase] Game {GameId} not found",
                    id);

                return GameResult.Fail(GameError.NotFound, "Game not found.");
            }

            await _gameRepository.DeleteAsync(game);
            await _gameRepository.SaveChangesAsync();

            _logger.LogInformation(
                "[App][DeleteGameUseCase] Game {GameId} deleted",
                id);

            return GameResult.Ok();
        }
    }
}
