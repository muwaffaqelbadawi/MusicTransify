using System;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Configurations.Spotify;
using MusicTransify.src.Services.Common;
using MusicTransify.src.Utilities.Common;

namespace MusicTransify.src.Controllers.Common
{
    [ApiController]
    public abstract class AuthController : ControllerBase
    {
        protected SpotifyOptionsProvider _spotifyOptionsProvider;
        protected readonly AuthService _authService;
        protected readonly SessionService _sessionService;
        protected readonly TokenHelper _token;
        protected readonly ILogger<AuthController> _logger;


        protected AuthController(SpotifyOptionsProvider spotifyOptionsProvider, AuthService authService, SessionService sessionService, TokenHelper tokenHelper, ILogger<AuthController> logger)
        {
            _spotifyOptionsProvider = spotifyOptionsProvider;
            _authService = authService;
            _sessionService = sessionService;
            _token = tokenHelper;
            _logger = logger;
        }
    }
}