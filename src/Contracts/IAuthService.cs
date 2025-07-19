using System;
using System.Text.Json;

namespace MusicTransify.src.Contracts
{
    public interface IAuthService
    {
        string GetLoginUri();
        Task<JsonElement> ExchangeAuthorizationCodeAsync(string authorizationCode);
        Task<JsonElement> GetNewTokenAsync(string refreshToken);
    }
}