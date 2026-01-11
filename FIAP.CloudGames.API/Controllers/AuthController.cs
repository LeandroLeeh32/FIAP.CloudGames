using FIAP.CloudGames.API.Controllers.DTOs.Requests.Auth;
using FIAP.CloudGames.Application.UseCases.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.CloudGames.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly LoginUserUseCase _useCase;

        public AuthController(LoginUserUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            var token = await _useCase.ExecuteAsync(
                request.Email,
                request.Password);

            return Ok(new { token });
        }
    }
}
