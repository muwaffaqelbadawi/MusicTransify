using System;
using System.Text.Json;

namespace MusicTransify.src.Contracts.YouTubeMusic
{
    public interface IYouTubeMusicService
    {
        string GetLoginUri();
        Task<JsonElement> ExchangeAuthorizationCodeAsync(string code);
        Task<JsonElement> GetNewTokenAsync(string refreshToken);
    }
}