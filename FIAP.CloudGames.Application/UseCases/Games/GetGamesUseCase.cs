using FIAP.CloudGames.Application.Repositories;

namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class GetGamesUseCase : IGetGamesUseCase
    {
        private readonly IGameRepository _gameRepository;

        public GetGamesUseCase(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<IReadOnlyList<GameDto>> ExecuteAsync()
        {
            var games = await _gameRepository.GetAllAsync();
            return games.Select(ToDto).ToList();
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
