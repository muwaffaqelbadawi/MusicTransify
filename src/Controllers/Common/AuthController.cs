using System;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Contracts;
using MusicTransify.src.Services.Common;

namespace MusicTransify.src.Controllers.Common
{
    [ApiController]
    public abstract class AuthController : ControllerBase, IAuthController
    {
        protected readonly AuthService _authService;
        protected readonly SessionService _sessionService;
        protected readonly ILogger<AuthController> _logger;
        protected AuthController(AuthService authService, SessionService sessionService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _sessionService = sessionService;
            _logger = logger;
        }

        public void Login(string service)
        {
            // Use the log information
            _logger.LogInformation("This is the Home route");

            string htmlContent = "<html>" +
            "<body>" +
            $"<h1>Welcome to {service} App</h1>" +
            $"<a href='/auth/login/spotify'>Login with {service}</a>" +
            "</body>" +
            "</html>";

            _logger.LogInformation("This is the Login route");

            Content(htmlContent, "text/html");
        }
    }
}