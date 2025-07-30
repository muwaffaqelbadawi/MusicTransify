using System;

namespace MusicTransify.src.Contracts.DTOs.Spotify
{
    public record RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string GrantType { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;

        public Dictionary<string, string> ToDictionary() => new()
        {
            ["grant_type"] = GrantType,
            ["refresh_token"] = RefreshToken,
            ["client_id"] = ClientId,
            ["client_secret"] = ClientSecret,
        };        
    }
}