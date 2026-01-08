using FIAP.CloudGames.Application.UseCases.ProcessGame;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.CloudGames.API.Controllers
{
    [ApiController]
    [Authorize]

    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {

        private readonly IProcessGameUseCase _processGame;

        public GamesController(IProcessGameUseCase processGame)
        {
            _processGame = processGame;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("ProcessGame")]
        public async Task<IActionResult> ProcessGame([FromBody] ProcessGameRequest request)
        {
            var result = await _processGame.ExecuteAsync(request);
            return Ok(result);
        }
    }
}
