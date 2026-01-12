using FIAP.CloudGames.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class GetGamesUseCase
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<GetGamesUseCase> _logger;

        public GetGamesUseCase(
            IGameRepository gameRepository,
            ILogger<GetGamesUseCase> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<IReadOnlyList<GameDto>> ExecuteAsync()
        {
            _logger.LogInformation("[App][GetGamesUseCase] Fetching games list");

            var games = await _gameRepository.GetAllAsync();
            var result = games.Select(ToDto).ToList();

            _logger.LogInformation(
                "[App][GetGamesUseCase] Games fetched. Total: {Total}",
                result.Count);

            return result;
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
