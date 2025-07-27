using System;
using System.Text.Json;

namespace MusicTransify.src.Contracts.Services
{
    public interface IProviderService
    {
        string GetLoginUri();
        Task<JsonElement> ExchangeAuthorizationCodeAsync(string code);
        Task<JsonElement> GetNewTokenAsync(string refreshToken);
    }
}