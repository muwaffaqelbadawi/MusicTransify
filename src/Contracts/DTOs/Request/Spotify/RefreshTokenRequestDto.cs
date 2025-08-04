using System;
using System.Text.Json.Serialization;
using MusicTransify.src.Contracts.Mapper;


namespace MusicTransify.src.Contracts.DTOs.Request.Spotify
{
    public class RefreshTokenRequestDto : IMappable
    {
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; } = string.Empty;
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = string.Empty;
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; } = string.Empty;

        public Dictionary<string, string> ToMap() => new()
        {
            ["grant_type"] = GrantType,
            ["refresh_token"] = RefreshToken,
            ["client_id"] = ClientId,
            ["client_secret"] = ClientSecret,
        };        
    }
}