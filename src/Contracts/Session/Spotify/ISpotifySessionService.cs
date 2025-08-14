using System;
using MusicTransify.src.Api.Endpoints.DTOs.Responses.Token.Spotify;

namespace MusicTransify.src.Contracts.Session.Spotify
{
    public interface ISpotifySessionService
    {
        public void StoreState(string state);

        public bool ValidateState(string state);

        public (
            string accessToken,
            string tokenType,
            int expiresIn,
            string refreshToken,
            string scope
        ) ExtractTokenData(SpotifyTokenResponseDto tokenInfo);

        public void Store(SpotifyTokenResponseDto tokenInfo);

        public bool IsSessionValid();

        public string? GetProtectedToken(string tokenKey);

        string? GetAccessToken();

        string? GetRefreshToken();
        
        DateTime? GetExpiration();

        public string? GetTokenInfo(string tokenInfo);
    }
}