using System;
using MusicTransify.src.Api.YouTubeMusic.Token.Responses;

namespace MusicTransify.src.Contracts.Services.Auth.YouTubeMusic
{
    public interface IYouTubeMusicService
    {
        string GetLoginUri();
        Task<YouTubeMusicTokenResponseDto> ExchangeAuthorizationCodeAsync(string code);
        Task<YouTubeMusicTokenResponseDto> GetNewTokenAsync(string refreshToken);
    }
}