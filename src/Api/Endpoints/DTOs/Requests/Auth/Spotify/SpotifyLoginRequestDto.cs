using System;
using System.Text.Json.Serialization;
using MusicTransify.src.Contracts.Mapper;

namespace MusicTransify.src.Api.Endpoints.DTOs.Requests.Auth.Spotify
{
    public class SpotifyLoginRequestDto : IMappable
    {
        [JsonPropertyName("response_type")]
        public string? ResponseType { get; set; }
        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; }
        [JsonPropertyName("scope")]
        public string? Scope { get; set; }
        [JsonPropertyName("redirect_uri")]
        public string? RedirectUri { get; set; }
        [JsonPropertyName("show_dialog")]
        public string? ShowDialog { get; set; }
        [JsonPropertyName("state")]
        public string? State { get; set; }

        public Dictionary<string, string> ToMap() => new()
        {
            ["response_type"] = ResponseType ?? string.Empty,
            ["client_id"] = ClientId ?? string.Empty,
            ["scope"] = Scope ?? string.Empty,
            ["redirect_uri"] = RedirectUri ?? string.Empty,
            ["show_dialog"] = ShowDialog ?? string.Empty,
            ["state"] = State ?? string.Empty
        };
    }
}