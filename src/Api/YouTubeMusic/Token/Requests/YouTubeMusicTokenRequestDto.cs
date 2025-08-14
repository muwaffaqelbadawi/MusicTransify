using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Api.YouTubeMusic.Token.Requests
{
    public class YouTubeMusicTokenRequestDto
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; }
        [JsonPropertyName("client_secret")]
        public string? ClientSecret { get; set; }
        [JsonPropertyName("redirect_uri")]
        public string? RedirectUri { get; set; }
        [JsonPropertyName("grant_type")]
        public string? GrantType { get; set; }

        public Dictionary<string, string> ToDictionary() => new()
        {
            ["code"] = Code ?? string.Empty,
            ["client_id"] = ClientId ?? string.Empty,
            ["client_secret"] = ClientSecret ?? string.Empty,
            ["redirect_uri"] = RedirectUri ?? string.Empty,
            ["grant_type"] = GrantType ?? string.Empty,
        };
    }
}