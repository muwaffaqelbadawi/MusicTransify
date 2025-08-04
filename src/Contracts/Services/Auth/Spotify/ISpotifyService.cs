using System;
using MusicTransify.src.Contracts.DTOs.Response.Token.Spotify;

namespace MusicTransify.src.Contracts.Services.Auth.Spotify
{
    public interface ISpotifyService
    {
        string GetLoginUri();
        Task<SpotifyTokenResponse> ExchangeAuthorizationCodeAsync(string code);
        Task<SpotifyTokenResponse> GetNewTokenAsync(string refreshToken);
    }
}