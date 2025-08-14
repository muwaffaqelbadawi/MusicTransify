using System;
using System.Text.Json.Serialization;
using MusicTransify.src.Contracts.Mapper;


namespace MusicTransify.src.Api.Spotify.Token.Requests
{
    public class SpotifyNewTokenRequestDto : IMappable
    {
        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
        [JsonPropertyName("grant_type")]
        public string? GrantType { get; set; }
        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; }
        [JsonPropertyName("client_secret")]
        public string? ClientSecret { get; set; }

        public Dictionary<string, string> ToMap() => new()
        {
            ["grant_type"] = GrantType ?? string.Empty,
            ["refresh_token"] = RefreshToken ?? string.Empty,
            ["client_id"] = ClientId ?? string.Empty,
            ["client_secret"] = ClientSecret ?? string.Empty,
        };
    }
}