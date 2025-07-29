using System;
using System.Text.Json;

namespace MusicTransify.src.Contracts.Services.ProviderAuth.Spotify
{
    public interface ISpotifyService
    {
        string GetLoginUri();
        Task<JsonElement> ExchangeAuthorizationCodeAsync(string code);
        Task<JsonElement> GetNewTokenAsync(string refreshToken);
    }
}