using FIAP.CloudGames.API.Controllers.DTOs.Requests.Auth;
using FIAP.CloudGames.Application.UseCases.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.CloudGames.API.Controllers
{
    /// <summary>
    /// Authentication endpoints.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly LoginUserUseCase _useCase;

        public AuthController(LoginUserUseCase useCase)
        {
            _useCase = useCase;
        }

        /// <summary>
        /// Login and receive a JWT token.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
