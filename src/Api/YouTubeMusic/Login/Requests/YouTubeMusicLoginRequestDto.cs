using System;
using System.Text.Json.Serialization;
using MusicTransify.src.Contracts.Mapper;

namespace MusicTransify.src.Api.YouTubeMusic.Login.Requests
{
    public class YouTubeMusicLoginRequestDto : IMappable
    {
        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; }
        [JsonPropertyName("redirect_uri")]
        public string? RedirectUri { get; set; }
        [JsonPropertyName("response_type")]
        public string? ResponseType { get; set; }
        [JsonPropertyName("scope")]
        public string? Scope { get; set; }
        [JsonPropertyName("access_type")]
        public string? AccessType { get; set; }
        [JsonPropertyName("include_granted_scopes")]
        public string? IncludeGrantedScopes { get; set; }
        [JsonPropertyName("state")]
        public string? State { get; set; }
        public Dictionary<string, string> ToMap() => new()
        {
            ["client_id"] = ClientId ?? string.Empty,
            ["redirect_uri"] = RedirectUri ?? string.Empty,
            ["response_type"] = ResponseType ?? string.Empty,
            ["scope"] = Scope ?? string.Empty,
            ["access_type"] = AccessType ?? string.Empty,
            ["include_granted_scopes"] = IncludeGrantedScopes ?? string.Empty,
            ["state"] = State ?? string.Empty
        };
    }
}