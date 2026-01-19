using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class CreateGameUseCase
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<CreateGameUseCase> _logger;

        public CreateGameUseCase(
            IGameRepository gameRepository,
            ILogger<CreateGameUseCase> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<GameResult<GameDto>> ExecuteAsync(CreateGameInput input)
        {
            _logger.LogInformation(
                "[App][CreateGameUseCase] Starting create for title {Title}",
                input.Title);

            var validation = ValidateInput(input.Title, input.Price);
            if (!validation.Success)
            {
                _logger.LogWarning(
                    "[App][CreateGameUseCase] Validation failed for title {Title}",
                    input.Title);

                return GameResult<GameDto>.Fail(validation.Error, validation.Message ?? "Invalid data.");
            }

            var game = new Game(
                input.Title,
                input.Price,
                input.Description
            );

            await _gameRepository.AddAsync(game);
            await _gameRepository.SaveChangesAsync();

            _logger.LogInformation(
                "[App][CreateGameUseCase] Game created with id {GameId}",
                game.Id);

            return GameResult<GameDto>.Ok(ToDto(game));
        }

        private static GameDto ToDto(Game game)
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
