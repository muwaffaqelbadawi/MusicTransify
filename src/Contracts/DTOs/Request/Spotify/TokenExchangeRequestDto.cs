using System;
using System.Text.Json.Serialization;
using MusicTransify.src.Contracts.Mapper;


namespace MusicTransify.src.Contracts.DTOs.Request.Spotify
{
    public class TokenExchangeRequestDto : IMappable
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; } = string.Empty;
        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; } = string.Empty;
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = string.Empty;
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; } = string.Empty;

        public Dictionary<string, string> ToMap() => new()
        {
            ["code"] = Code,
            ["grant_type"] = GrantType,
            ["redirect_uri"] = RedirectUri,
            ["client_id"] = ClientId,
            ["client_secret"] = ClientSecret,
        };
    }
}