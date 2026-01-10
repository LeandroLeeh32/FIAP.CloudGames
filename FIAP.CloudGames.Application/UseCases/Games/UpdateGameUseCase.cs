using FIAP.CloudGames.Application.Repositories;

namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class UpdateGameUseCase
    {
        private readonly IGameRepository _gameRepository;

        public UpdateGameUseCase(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<GameResult<GameDto>> ExecuteAsync(Guid id, UpdateGameInput input)
        {
            var validation = ValidateInput(input.Title, input.Price);
            if (!validation.Success)
            {
                return GameResult<GameDto>.Fail(validation.Error, validation.Message ?? "Invalid data.");
            }

            var game = await _gameRepository.GetByIdAsync(id);
            if (game is null)
            {
                return GameResult<GameDto>.Fail(GameError.NotFound, "Game not found.");
            }

            game.Title = input.Title.Trim();
            game.Description = input.Description;
            game.Price = input.Price;

            await _gameRepository.UpdateAsync(game);
            await _gameRepository.SaveChangesAsync();

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
