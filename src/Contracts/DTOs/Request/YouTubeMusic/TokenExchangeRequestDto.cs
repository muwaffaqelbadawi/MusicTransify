using System;

namespace MusicTransify.src.Contracts.DTOs.Request.YouTubeMusic
{
    public class TokenExchangeRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public string GrantType { get; set; } = string.Empty;

        public Dictionary<string, string> ToDictionary() => new()
        {
            ["code"] = Code,
            ["client_id"] = ClientId,
            ["client_secret"] = ClientSecret,
            ["redirect_uri"] = RedirectUri,
            ["grant_type"] = GrantType,
        };
    }
}