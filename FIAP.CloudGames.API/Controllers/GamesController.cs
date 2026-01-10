using FIAP.CloudGames.API.Controllers.DTOs.Requests.Games;
using FIAP.CloudGames.API.Controllers.DTOs.Responses.Games;
using FIAP.CloudGames.Application.UseCases.Games;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.CloudGames.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGetGamesUseCase _getGamesUseCase;
        private readonly IGetGameByIdUseCase _getGameByIdUseCase;
        private readonly ICreateGameUseCase _createGameUseCase;
        private readonly IUpdateGameUseCase _updateGameUseCase;
        private readonly IDeleteGameUseCase _deleteGameUseCase;

        public GamesController(
            IGetGamesUseCase getGamesUseCase,
            IGetGameByIdUseCase getGameByIdUseCase,
            ICreateGameUseCase createGameUseCase,
            IUpdateGameUseCase updateGameUseCase,
            IDeleteGameUseCase deleteGameUseCase)
        {
            _getGamesUseCase = getGamesUseCase;
            _getGameByIdUseCase = getGameByIdUseCase;
            _createGameUseCase = createGameUseCase;
            _updateGameUseCase = updateGameUseCase;
            _deleteGameUseCase = deleteGameUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GameResponse>>> GetAll()
        {
            var games = await _getGamesUseCase.ExecuteAsync();
            var response = games.Select(MapToResponse).ToList();

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GameResponse>> GetById(Guid id)
        {
            var game = await _getGameByIdUseCase.ExecuteAsync(id);
            if (game is null)
            {
                return NotFound();
            }

            return Ok(MapToResponse(game));
        }

        [HttpPost]
        public async Task<ActionResult<GameResponse>> Create([FromBody] CreateGameRequest request)
        {
            var result = await _createGameUseCase.ExecuteAsync(new CreateGameInput
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price
            });

            if (!result.Success)
            {
                //log error
            }

            var response = MapToResponse(result.Data!);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<GameResponse>> Update(Guid id, [FromBody] UpdateGameRequest request)
        {
            var result = await _updateGameUseCase.ExecuteAsync(id, new UpdateGameInput
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price
            });

            if (!result.Success)
            {
                //log error
            }

            return Ok(MapToResponse(result.Data!));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _deleteGameUseCase.ExecuteAsync(id);
            if (!result.Success)
            {
                //log error
            }

            return NoContent();
        }

        private static GameResponse MapToResponse(GameDto game)
        {
            return new GameResponse
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price
            };
        }
    }
}
