using System;

namespace MusicTransify.src.Contracts.DTOs.Spotify
{
    public record TokenExchangeRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string GrantType { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;


        public Dictionary<string, string> ToDictionary() => new()
        {
            ["code"] = Code,
            ["grant_type"] = GrantType,
            ["redirect_uri"] = RedirectUri,
            ["client_id"] = ClientId,
            ["client_secret"] = ClientSecret,
        };
    }
}