using System;

namespace MusicTransify.src.Contracts.DTOs.YouTubeMusic
{
    public record RefreshTokenRequestDto
    {
        public string RefreshToken { get; init; } = string.Empty;
        public string ClientId { get; init; } = string.Empty;
        public string ClientSecret { get; init; } = string.Empty;
        public string RefreshTokenGrantType { get; init; } = string.Empty;

        public Dictionary<string, string> ToDictionary() => new()
        {
            ["refresh_token"] = RefreshToken,
            ["client_id"] = ClientId,
            ["client_secret"] = ClientSecret,
            ["grant_type"] = RefreshTokenGrantType
        };
    }
}