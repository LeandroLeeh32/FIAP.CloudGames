using FIAP.CloudGames.API.Controllers.DTOs.Requests.Games;
using FIAP.CloudGames.API.Controllers.DTOs.Responses.Games;
using FIAP.CloudGames.Application.UseCases.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.CloudGames.API.Controllers
{
    /// <summary>
    /// Endpoints for managing games.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GamesController : ControllerBase
    {
        private readonly GetGamesUseCase _getGamesUseCase;
        private readonly GetGameByIdUseCase _getGameByIdUseCase;
        private readonly CreateGameUseCase _createGameUseCase;
        private readonly UpdateGameUseCase _updateGameUseCase;
        private readonly DeleteGameUseCase _deleteGameUseCase;

        public GamesController(
            GetGamesUseCase getGamesUseCase,
            GetGameByIdUseCase getGameByIdUseCase,
            CreateGameUseCase createGameUseCase,
            UpdateGameUseCase updateGameUseCase,
            DeleteGameUseCase deleteGameUseCase)
        {
            _getGamesUseCase = getGamesUseCase;
            _getGameByIdUseCase = getGameByIdUseCase;
            _createGameUseCase = createGameUseCase;
            _updateGameUseCase = updateGameUseCase;
            _deleteGameUseCase = deleteGameUseCase;
        }

        /// <summary>
        /// List all games.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<GameResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IReadOnlyList<GameResponse>>> GetAll()
        {
            var games = await _getGamesUseCase.ExecuteAsync();
            var response = games.Select(MapToResponse).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Get a game by id.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(GameResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<GameResponse>> GetById(Guid id)
        {
            var game = await _getGameByIdUseCase.ExecuteAsync(id);
            if (game is null)
            {
                return NotFound();
            }

            return Ok(MapToResponse(game));
        }

        /// <summary>
        /// Create a new game.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(GameResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
                return result.Error switch
                {
                    GameError.Validation => BadRequest(result.Message),
                    GameError.NotFound => NotFound(result.Message),
                    _ => BadRequest(result.Message)
                };
            }

            var response = MapToResponse(result.Data!);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Update a game by id.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(GameResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
                return result.Error switch
                {
                    GameError.NotFound => NotFound(result.Message),
                    GameError.Validation => BadRequest(result.Message),
                    _ => BadRequest(result.Message)
                };
            }

            return Ok(MapToResponse(result.Data!));
        }

        /// <summary>
        /// Delete a game by id.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _deleteGameUseCase.ExecuteAsync(id);
            if (!result.Success)
            {
                return result.Error switch
                {
                    GameError.NotFound => NotFound(result.Message),
                    _ => BadRequest(result.Message)
                };
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
