using System;
using MusicTransify.src.Contracts.Session.Spotify;

namespace MusicTransify.src.Utilities.Session.Spotify
{
    public class SpotifyTokenInfoHelper
    {
        private readonly ISpotifySessionService _sessionService;

        public SpotifyTokenInfoHelper(
            ISpotifySessionService sessionService
        )
        {
            _sessionService = sessionService;
        }

        public string AccessToken => _sessionService.GetTokenInfo("access_token") ?? string.Empty;
        public string TokenType => _sessionService.GetTokenInfo("token_type") ?? string.Empty;
        public string RefreshToken => _sessionService.GetTokenInfo("refreshToken") ?? string.Empty;
        public string ExpiresIn => _sessionService.GetTokenInfo("expires_in") ?? string.Empty;
        public string Scope => _sessionService.GetTokenInfo("scope") ?? string.Empty;
    }
}