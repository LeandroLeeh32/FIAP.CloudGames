using FIAP.CloudGames.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class UpdateGameUseCase
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<UpdateGameUseCase> _logger;

        public UpdateGameUseCase(
            IGameRepository gameRepository,
            ILogger<UpdateGameUseCase> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<GameResult<GameDto>> ExecuteAsync(Guid id, UpdateGameInput input)
        {
            _logger.LogInformation(
                "[App][UpdateGameUseCase] Starting update for game {GameId}",
                id);

            var validation = ValidateInput(input.Title, input.Price);
            if (!validation.Success)
            {
                _logger.LogWarning(
                    "[App][UpdateGameUseCase] Validation failed for game {GameId}",
                    id);

                return GameResult<GameDto>.Fail(validation.Error, validation.Message ?? "Invalid data.");
            }

            var game = await _gameRepository.GetByIdAsync(id);
            if (game is null)
            {
                _logger.LogWarning(
                    "[App][UpdateGameUseCase] Game {GameId} not found",
                    id);

                return GameResult<GameDto>.Fail(GameError.NotFound, "Game not found.");
            }

            game.UpdateTitle(input.Title);
            game.UpdateDescription(input.Description);
            game.UpdatePrice(input.Price);

            await _gameRepository.UpdateAsync(game);
            await _gameRepository.SaveChangesAsync();

            _logger.LogInformation(
                "[App][UpdateGameUseCase] Game {GameId} updated",
                id);

            return GameResult<GameDto>.Ok(ToDto(game));
        }

        private static GameDto ToDto(Domain.Entities.Game game)
        {
            return new GameDto
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price
            };
        }

        private static GameResult ValidateInput(string title, decimal price)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return GameResult.Fail(GameError.Validation, "Title is required.");
            }

            if (price < 0)
            {
                return GameResult.Fail(GameError.Validation, "Price must be zero or greater.");
            }

            return GameResult.Ok();
        }
    }
}
