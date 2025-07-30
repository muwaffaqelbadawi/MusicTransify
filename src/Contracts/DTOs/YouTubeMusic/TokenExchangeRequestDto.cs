using System;

namespace MusicTransify.src.Contracts.DTOs.YouTubeMusic
{
    public record TokenExchangeRequestDto
    {
        public string Code { get; init; } = string.Empty;
        public string ClientId { get; init; } = string.Empty;
        public string ClientSecret { get; init; } = string.Empty;
        public string RedirectUri { get; init; } = string.Empty;
        public string GrantType { get; init; } = string.Empty;

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