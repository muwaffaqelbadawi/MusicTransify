using System;
using Microsoft.AspNetCore.Mvc;

namespace MusicTransify.src.Controllers.Home
{
    [ApiController]
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            _logger.LogInformation("Health check hit");
            return Ok("API is alive");
        }
    }
}
