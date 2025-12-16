using Microsoft.AspNetCore.Mvc;

namespace FIAP.CloudGames.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestLogController : ControllerBase
    {
        private readonly ILogger<TestLogController> _logger;

        public TestLogController(ILogger<TestLogController> logger)
        {
                _logger = logger;
        }

        [HttpGet]
        public ActionResult Get() {

            _logger.LogInformation("Teste de log funcionando!");
            return Ok("Log gerado.veja a pasta /logs");
        }
    }
}
