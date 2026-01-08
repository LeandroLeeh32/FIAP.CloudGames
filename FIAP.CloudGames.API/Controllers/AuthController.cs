using FIAP.CloudGames.Application.UseCases.Authentication;
using Microsoft.AspNetCore.Http;
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
        public IActionResult Login([FromBody] string email)
        {
            var token = _useCase.Execute(email);
            return Ok(new { token });
        }
    }
}
