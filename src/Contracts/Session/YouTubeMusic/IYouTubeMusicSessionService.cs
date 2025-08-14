using System;
using MusicTransify.src.Api.Endpoints.DTOs.Responses.Token.YouTubeMusic;

namespace MusicTransify.src.Contracts.Session.YouTubeMusic
{
    public interface IYouTubeMusicSessionService
    {
        public void StoreState(string state);

        public bool ValidateState(string state);

        public void Store(YouTubeMusicTokenResponseDto tokenInfo);

        DateTime? GetExpiration();

        public string? GetTokenInfo(string tokenInfo);
    }
}