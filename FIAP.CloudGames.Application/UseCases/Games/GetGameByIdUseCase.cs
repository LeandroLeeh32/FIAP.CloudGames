using FIAP.CloudGames.Application.Repositories;

namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class GetGameByIdUseCase
    {
        private readonly IGameRepository _gameRepository;

        public GetGameByIdUseCase(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<GameDto?> ExecuteAsync(Guid id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            return game is null ? null : ToDto(game);
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
