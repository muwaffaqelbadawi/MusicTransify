using System;
using System.Text.Json.Serialization;
using MusicTransify.src.Contracts.Mapper;

namespace MusicTransify.src.Contracts.DTOs.Request.YouTubeMusic
{
    public class LoginRequestDto : IMappable
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = string.Empty;
        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; } = string.Empty;
        [JsonPropertyName("response_type")]
        public string ResponseType { get; set; } = string.Empty;
        [JsonPropertyName("scope")]
        public string Scope { get; set; } = string.Empty;
        [JsonPropertyName("access_type")]
        public string AccessType { get; set; } = string.Empty;
        [JsonPropertyName("include_granted_scopes")]
        public string IncludeGrantedScopes { get; set; } = string.Empty;
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        public Dictionary<string, string> ToMap() => new()
        {
            ["client_id"] = ClientId,
            ["redirect_uri"] = RedirectUri,
            ["response_type"] = ResponseType,
            ["scope"] = Scope,
            ["access_type"] = AccessType,
            ["include_granted_scopes"] = IncludeGrantedScopes,
            ["state"] = State
        };
    }
}