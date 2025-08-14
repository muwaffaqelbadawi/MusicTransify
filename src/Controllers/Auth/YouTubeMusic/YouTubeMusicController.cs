using System;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Api.YouTubeMusic.Callback.Responses;
using MusicTransify.src.Contracts.Services.Auth.YouTubeMusic;
using MusicTransify.src.Contracts.Session.YouTubeMusic;
using MusicTransify.src.Utilities.Session.YouTubeMusic;
using MusicTransify.src.Utilities.Token;

namespace MusicTransify.src.Controllers.Auth.YouTubeMusic
{
    [ApiController]
    [Route("api/youtube")]
    public class YouTubeMusicController : Controller
    {
        private readonly IYouTubeMusicService _youTubeMusicService;
        private readonly IYouTubeMusicSessionService _sessionService;
        private readonly YouTubeMusicTokenInfoHelper _tokenInfo;
        private readonly TokenHelper _tokenHelper;
        private readonly ILogger<YouTubeMusicController> _logger;
        public YouTubeMusicController(
            IYouTubeMusicService youTubeMusicService,
            YouTubeMusicTokenInfoHelper tokenInfo,
            IYouTubeMusicSessionService sessionService,
            TokenHelper tokenHelper,
            ILogger<YouTubeMusicController> logger
        )
        {
            _youTubeMusicService = youTubeMusicService;
            _tokenInfo = tokenInfo;
            _sessionService = sessionService;
            _tokenHelper = tokenHelper;
            _logger = logger;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            _logger.LogInformation("This is YouTube Music Login route");

            // Set redirect URI
            string redirectUri = _youTubeMusicService.GetLoginUri();

            _logger.LogInformation("Redirecting to YouTube Music login: {RedirectUri}", redirectUri);

            return Redirect(redirectUri);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> CallbackAsync([FromQuery] YouTubeMusicCallbackResponseDto response)
        {
            _logger.LogInformation("This is the callback route");

            var validationResult = ValidateCallbackResponse(response);
            if (validationResult is not null) return validationResult;

            if (string.IsNullOrEmpty(response.Code))
            {
                _logger.LogWarning("Authorization code is missing.");
                return BadRequest("Authorization code is missing.");
            }

            var tokenInfo = await _youTubeMusicService.ExchangeAuthorizationCodeAsync(response.Code);

            _logger.LogInformation("Receiving the token info");
            _logger.LogInformation("Access Token: {AccessToken}", tokenInfo.AccessToken);
            
            if (tokenInfo is null)
            {
                _logger.LogError("Failed to exchange authorization code and getting the token info.");

                return BadRequest("Failed to exchange authorization code and getting the token info.");
            }

            _logger.LogInformation("Token info successfully received.");
            
            // Store the token info in session
            _sessionService.Store(tokenInfo);

            _logger.LogInformation("Token info successfully stored in session redirecting to playlist route...");

            // return Redirect("http://localhost:3000/playlists/youtube");
            return Redirect("/api/youtube/playlists");
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            _logger.LogInformation("This is the refreshToken route");

            // Set access token
            string accessToken = _tokenInfo.AccessToken;
            string strExpiresIn = _tokenInfo.ExpiresIn;
            string refreshToken = _tokenInfo.RefreshToken;

            if (string.IsNullOrEmpty(strExpiresIn))
            {
                _logger.LogWarning("The 'expires_in' parameter is missing or invalid in the session.");
                return BadRequest("The 'expires_in' parameter is missing or invalid in the session.");
            }

            long expiresIn = _tokenHelper.ParseToLong(strExpiresIn);

            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Missing accessToken parameter from session. Redirecting to login from YouTubeMusicController...");

                // This must be changed to a resolved URL
                return Redirect("/api/youtube/login");
            }

            if (_tokenHelper.IsExpired(expiresIn))
            {
                if (string.IsNullOrEmpty(refreshToken))
                {
                    _logger.LogWarning("Refresh token missing from session.");

                    return BadRequest("The 'refreshToken' parameter is missing or invalid in the session.");
                }

                var newAccessToken = await _youTubeMusicService.GetNewTokenAsync(refreshToken);

                if (newAccessToken is null)
                {
                    _logger.LogError("Failed to get new token using refresh token.");

                    return BadRequest("Failed to get new token");
                }

                // Store the new token in session
                _sessionService.Store(newAccessToken);

                _logger.LogInformation("Token successfully refreshed and stored in session. Redirecting...");

                return Ok("Access token granted for YouTube Auth access and stored successfully. You can now access your playlists.");
            }

            _logger.LogInformation("Token is still valid, no refresh needed.");

            return Ok("Token is still valid, no refresh needed.");
        }

        private BadRequestObjectResult? ValidateCallbackResponse(YouTubeMusicCallbackResponseDto response)
        {
            if (string.IsNullOrEmpty(response.Code))
            {
                _logger.LogWarning("Missing 'Code' parameter in callback request.");
                return BadRequest("Missing 'Code' parameter in callback request.");
            }

            if (string.IsNullOrEmpty(response.State))
            {
                _logger.LogWarning("Missing 'State' parameter in callback (optional).");
                return BadRequest("Missing 'State' parameter in callback request.");
            }

            if (string.IsNullOrEmpty(response.Scope))
            {
                _logger.LogWarning("Missing 'Scope' parameter in callback request.");
                return BadRequest("Missing 'Scope' parameter in callback request.");
            }

            if (!string.IsNullOrEmpty(response.Error))
            {
                _logger.LogWarning("OAuth error in callback: {Error}", response.Error);
            }

            return null!;
        }
    }
}