using System;

namespace MusicTransify.src.Contracts.DTOs.YouTubeMusic
{
    public record LoginRequestDto
    {
        public string ClientId { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public string ResponseType { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string AccessType { get; set; } = string.Empty;
        public string IncludeGrantedScopes { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;

        public Dictionary<string, string> ToDictionary() => new()
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