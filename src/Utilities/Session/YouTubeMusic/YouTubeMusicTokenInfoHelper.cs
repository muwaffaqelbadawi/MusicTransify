using System;
using MusicTransify.src.Contracts.Session.YouTubeMusic;

namespace MusicTransify.src.Utilities.Session.YouTubeMusic
{
    public class YouTubeMusicTokenInfoHelper
    {
        private readonly IYouTubeMusicSessionService _sessionService;
        private readonly ILogger<YouTubeMusicTokenInfoHelper> _logger;

        public YouTubeMusicTokenInfoHelper(
            IYouTubeMusicSessionService sessionService,
            ILogger<YouTubeMusicTokenInfoHelper> logger
        )
        {
            _sessionService = sessionService;
            _logger = logger;
        }

        public string AccessToken => _sessionService.GetTokenInfo("access_token") ?? string.Empty;
        public string TokenType => _sessionService.GetTokenInfo("token_type") ?? string.Empty;
        public string RefreshToken => _sessionService.GetTokenInfo("refreshToken") ?? string.Empty;
        public string ExpiresIn => _sessionService.GetTokenInfo("expires_in") ?? string.Empty;
        public string Scope => _sessionService.GetTokenInfo("scope") ?? string.Empty;
    }
} 