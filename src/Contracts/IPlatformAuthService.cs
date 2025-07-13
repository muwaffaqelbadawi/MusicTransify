using System;
using System.Text.Json;

namespace MusicTransify.src.Contracts
{
    public interface IPlatformAuthService
    {
        string GetLoginUri();
        Task<JsonElement> ExchangeAuthorizationCodeAsync(string authorizationCode);
        Task<JsonElement> GetNewTokenAsync(string refreshToken);
    }
}