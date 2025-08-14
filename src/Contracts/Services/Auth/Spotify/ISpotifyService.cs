using System;
using MusicTransify.src.Api.Endpoints.DTOs.Responses.Token.Spotify;
namespace MusicTransify.src.Contracts.Services.Auth.Spotify
{
    public interface ISpotifyService
    {
        string GetLoginUri();
        Task<SpotifyTokenResponseDto> ExchangeAuthorizationCodeAsync(string code);
        Task<SpotifyTokenResponseDto> GetNewTokenAsync(string refreshToken);
    }
}