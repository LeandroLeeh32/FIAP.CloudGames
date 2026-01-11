using FIAP.CloudGames.Application.Repositories;

namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class DeleteGameUseCase
    {
        private readonly IGameRepository _gameRepository;

        public DeleteGameUseCase(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<GameResult> ExecuteAsync(Guid id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game is null)
            {
                return GameResult.Fail(GameError.NotFound, "Game not found.");
            }

            await _gameRepository.DeleteAsync(game);
            await _gameRepository.SaveChangesAsync();

            return GameResult.Ok();
        }
    }
}
