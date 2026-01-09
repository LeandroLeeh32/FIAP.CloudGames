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
        private readonly IGamesUseCase _gamesUseCase;

        public GamesController(IGamesUseCase gamesUseCase)
        {
            _gamesUseCase = gamesUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GameResponse>>> GetAll()
        {
            var games = await _gamesUseCase.GetAllAsync();
            var response = games.Select(MapToResponse).ToList();

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GameResponse>> GetById(Guid id)
        {
            var game = await _gamesUseCase.GetByIdAsync(id);
            if (game is null)
            {
                return NotFound();
            }

            return Ok(MapToResponse(game));
        }

        [HttpPost]
        public async Task<ActionResult<GameResponse>> Create([FromBody] CreateGameRequest request)
        {
            var result = await _gamesUseCase.CreateAsync(new CreateGameInput
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price
            });

            if (!result.Success)
            {
                 return BadRequest(result.Message);
            }

            var response = MapToResponse(result.Data!);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<GameResponse>> Update(Guid id, [FromBody] UpdateGameRequest request)
        {
            var result = await _gamesUseCase.UpdateAsync(id, new UpdateGameInput
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price
            });

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(MapToResponse(result.Data!));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _gamesUseCase.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
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
