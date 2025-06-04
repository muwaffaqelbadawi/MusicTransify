using System;
using System.Linq;
using System.Text.Json;

namespace MusicTransify.src.Services.Common
{
    public abstract class AuthService
    {
        abstract public string GetLogInURI();

        abstract public Task<JsonElement> ExchangeAuthorizationCodeAsync(string authorizationCode);
        abstract public Task<JsonElement> GetNewTokenAsync(string refreshToken);


    }
}