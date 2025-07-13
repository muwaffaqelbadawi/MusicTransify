using System;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Contracts;

namespace MusicTransify.src.Controllers.Auth.Common.Login
{
    [ApiController]
    [Route("login/{platform}")] // route "login/{platform}"
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly Func<string, IPlatformAuthService> _platformAuthFactory;
        public LoginController(
        ILogger<LoginController> Baselogger,
        Func<string, IPlatformAuthService> platformAuthFactory)
        {
            _platformAuthFactory = platformAuthFactory;
            _logger = Baselogger;
        }

        [HttpPost("")] // route "login/{platform}"
        public IActionResult Login(string platform)
        {
            _logger.LogInformation("This is the login route");
            var authService = _platformAuthFactory(platform);
            return Ok(authService.GetLoginUri());
        }

        [HttpPost("exchange")] // route "login/{platform}/exchange"
        public async Task<IActionResult> ExchangeCode(string platform, [FromBody] string code)
        {
            var authService = _platformAuthFactory(platform);
            var token = await authService.ExchangeAuthorizationCodeAsync(code);
            return Ok(token);
        }

        [HttpPost("refresh")] // route "login/{platform}/refresh"
        public async Task<IActionResult> RefreshToken(string platform, [FromBody] string refreshToken)
        {
            var authService = _platformAuthFactory(platform);
            var token = await authService.GetNewTokenAsync(refreshToken);
            return Ok(token);
        }
    }
}