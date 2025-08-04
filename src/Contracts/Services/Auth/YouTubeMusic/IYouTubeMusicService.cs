using System;
using MusicTransify.src.Contracts.DTOs.Response.Token.YouTubeMusic;

namespace MusicTransify.src.Contracts.Services.Auth.YouTubeMusic
{
    public interface IYouTubeMusicService
    {
        string GetLoginUri();
        Task<YouTubeMusicTokenResponse> ExchangeAuthorizationCodeAsync(string code);
        Task<YouTubeMusicTokenResponse> GetNewTokenAsync(string refreshToken);
    }
}