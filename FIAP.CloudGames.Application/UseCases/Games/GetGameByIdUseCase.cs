using FIAP.CloudGames.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class GetGameByIdUseCase
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<GetGameByIdUseCase> _logger;

        public GetGameByIdUseCase(
            IGameRepository gameRepository,
            ILogger<GetGameByIdUseCase> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<GameDto?> ExecuteAsync(Guid id)
        {
            _logger.LogInformation(
                "[App][GetGameByIdUseCase] Fetching game {GameId}",
                id);

            var game = await _gameRepository.GetByIdAsync(id);
            if (game is null)
            {
                _logger.LogWarning(
                    "[App][GetGameByIdUseCase] Game {GameId} not found",
                    id);

                return null;
            }

            return ToDto(game);
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
    }
}
