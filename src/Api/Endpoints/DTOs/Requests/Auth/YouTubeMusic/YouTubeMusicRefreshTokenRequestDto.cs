using System;
using System.Text.Json.Serialization;
using MusicTransify.src.Contracts.Mapper;

namespace MusicTransify.src.Api.Endpoints.DTOs.Requests.Auth.YouTubeMusic
{
    public class YouTubeMusicRefreshTokenRequestDto : IMappable
    {
        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; }
        [JsonPropertyName("client_secret")]
        public string? ClientSecret { get; set; }
        [JsonPropertyName("grant_type")]
        public string? RefreshTokenGrantType { get; set; }

        public Dictionary<string, string> ToMap() => new()
        {
            ["refresh_token"] = RefreshToken ?? string.Empty,
            ["client_id"] = ClientId ?? string.Empty,
            ["client_secret"] = ClientSecret ?? string.Empty,
            ["grant_type"] = RefreshTokenGrantType ?? string.Empty
        };
    }
}