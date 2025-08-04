using System;
using System.Text.Json.Serialization;
using MusicTransify.src.Contracts.Mapper;

namespace MusicTransify.src.Contracts.DTOs.Request.Spotify
{
    public class LoginRequestDto : IMappable
    {
        [JsonPropertyName("response_type")]
        public string ResponseType { get; set; } = string.Empty;
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = string.Empty;
        [JsonPropertyName("scope")]
        public string Scope { get; set; } = string.Empty;
        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; } = string.Empty;
        [JsonPropertyName("show_dialog")]
        public string ShowDialog { get; set; } = string.Empty;
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        public Dictionary<string, string> ToMap() => new()
        {
            ["response_type"] = ResponseType,
            ["client_id"] = ClientId,
            ["scope"] = Scope,
            ["redirect_uri"] = RedirectUri,
            ["show_dialog"] = ShowDialog,
            ["state"] = State
        };
    }
}