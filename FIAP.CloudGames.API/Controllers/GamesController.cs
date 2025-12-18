using FIAP.CloudGames.Application.UseCases.ProcessGame;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.CloudGames.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {

        private readonly IProcessGameUseCase _processGame;

        public GamesController(IProcessGameUseCase processGame)
        {
            _processGame = processGame;
        }

        [HttpPost("ProcessGame")]
        public async Task<IActionResult> ProcessGame([FromBody] ProcessGameRequest request)
        {
            var result = await _processGame.ExecuteAsync(request);
            return Ok(result);
        }
    }
}
