using MusicTransify.src.Contracts.DTOs.Response.Token.YouTubeMusic;
using System;

namespace MusicTransify.src.Contracts.Session.YouTubeMusic
{
    public interface IYouTubeMusicSessionService
    {
        public void StoreState(string state);

        public bool ValidateState(string state);

        public (
            string accessToken,
            string tokenType,
            int expiresIn,
            string refreshToken,
            string scope
        ) ExtractTokenData(YouTubeMusicTokenResponse tokenInfo);

        public void Store(YouTubeMusicTokenResponse tokenInfo);

        public void RegenerateSession();

        public bool IsSessionValid();

        public string? GetProtectedToken(string tokenKey);

        string? GetAccessToken();

        string? GetRefreshToken();

        DateTime? GetExpiration();

        public string? GetTokenInfo(string tokenInfo);
    }
}