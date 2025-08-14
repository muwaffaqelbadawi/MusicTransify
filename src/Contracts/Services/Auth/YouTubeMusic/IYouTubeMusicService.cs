using System;
using MusicTransify.src.Api.Endpoints.DTOs.Responses.Token.YouTubeMusic;

namespace MusicTransify.src.Contracts.Services.Auth.YouTubeMusic
{
    public interface IYouTubeMusicService
    {
        string GetLoginUri();
        Task<YouTubeMusicTokenResponseDto> ExchangeAuthorizationCodeAsync(string code);
        Task<YouTubeMusicTokenResponseDto> GetNewTokenAsync(string refreshToken);
    }
}