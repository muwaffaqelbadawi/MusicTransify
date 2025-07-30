using System;

namespace MusicTransify.src.Contracts.DTOs.Spotify
{
    public record LoginRequestDto
    {
        public string ResponseType { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
        public string ShowDialog { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;

        public Dictionary<string, string> ToDictionary() => new()
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