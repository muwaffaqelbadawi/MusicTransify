using System;
using System.Text.Json.Serialization;
using MusicTransify.src.Contracts.Mapper;


namespace MusicTransify.src.Api.Endpoints.DTOs.Requests.Auth.Spotify
{
    public class SpotifyTokenExchangeRequestDto : IMappable
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        [JsonPropertyName("grant_type")]
        public string? GrantType { get; set; }
        [JsonPropertyName("redirect_uri")]
        public string? RedirectUri { get; set; }
        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; }
        [JsonPropertyName("client_secret")]
        public string? ClientSecret { get; set; }

        public Dictionary<string, string> ToMap() => new()
        {
            ["code"] = Code ?? string.Empty,
            ["grant_type"] = GrantType ?? string.Empty,
            ["redirect_uri"] = RedirectUri ?? string.Empty,
            ["client_id"] = ClientId ?? string.Empty,
            ["client_secret"] = ClientSecret ?? string.Empty,
        };
    }
}