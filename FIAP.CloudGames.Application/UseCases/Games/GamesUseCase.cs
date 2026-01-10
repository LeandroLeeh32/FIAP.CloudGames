using FIAP.CloudGames.Domain.Entities;

namespace FIAP.CloudGames.Application.UseCases.Games
{
    public class GamesUseCase : IGamesUseCase
    {
        private readonly ApplicationDbContext _dbContext;

        public GamesUseCase(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<GameDto>> GetAllAsync()
        {
            return await _dbContext.Games
                .AsNoTracking()
                .OrderBy(g => g.Title)
                .Select(g => new GameDto
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                    Price = g.Price
                })
                .ToListAsync();
        }

        public async Task<GameDto?> GetByIdAsync(Guid id)
        {
            var game = await _dbContext.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);

            return game is null ? null : ToDto(game);
        }

        public async Task<GameResult<GameDto>> CreateAsync(CreateGameInput input)
        {
            var validation = ValidateInput(input.Title, input.Price);
            if (!validation.Success)
            {
                return GameResult<GameDto>.Fail(validation.Error, validation.Message ?? "Invalid data.");
            }

            var game = new Game
            {
                Id = Guid.NewGuid(),
                Title = input.Title.Trim(),
                Description = input.Description,
                Price = input.Price
            };

            _dbContext.Games.Add(game);
            await _dbContext.SaveChangesAsync();

            return GameResult<GameDto>.Ok(ToDto(game));
        }

        public async Task<GameResult<GameDto>> UpdateAsync(Guid id, UpdateGameInput input)
        {
            var validation = ValidateInput(input.Title, input.Price);
            if (!validation.Success)
            {
                return GameResult<GameDto>.Fail(validation.Error, validation.Message ?? "Invalid data.");
            }

            var game = await _dbContext.Games.FindAsync(id);
            if (game is null)
            {
                return GameResult<GameDto>.Fail(GameError.NotFound, "Game not found.");
            }

            game.Title = input.Title.Trim();
            game.Description = input.Description;
            game.Price = input.Price;

            await _dbContext.SaveChangesAsync();

            return GameResult<GameDto>.Ok(ToDto(game));
        }

        public async Task<GameResult> DeleteAsync(Guid id)
        {
            var game = await _dbContext.Games.FindAsync(id);
            if (game is null)
            {
                return GameResult.Fail(GameError.NotFound, "Game not found.");
            }

            _dbContext.Games.Remove(game);
            await _dbContext.SaveChangesAsync();

            return GameResult.Ok();
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
