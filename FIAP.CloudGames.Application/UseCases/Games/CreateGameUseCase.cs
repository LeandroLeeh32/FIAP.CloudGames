using FIAP.CloudGames.Domain.Entities;
using FIAP.CloudGames.Application.Repositories;

namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class CreateGameUseCase
    {
        private readonly IGameRepository _gameRepository;

        public CreateGameUseCase(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<GameResult<GameDto>> ExecuteAsync(CreateGameInput input)
        {
            var validation = ValidateInput(input.Title, input.Price);
            if (!validation.Success)
            {
                return GameResult<GameDto>.Fail(validation.Error, validation.Message ?? "Invalid data.");
            }

            var game = new Game(
                input.Title,
                input.Price,
                input.Description
            );

            //var game = new Game
            //{
            //    Id = Guid.NewGuid(),
            //    Title = input.Title.Trim(),
            //    Description = input.Description,
            //    Price = input.Price
            //};

            await _gameRepository.AddAsync(game);
            await _gameRepository.SaveChangesAsync();

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
