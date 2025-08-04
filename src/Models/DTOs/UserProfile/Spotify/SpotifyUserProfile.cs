using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Models.DTOs.UserProfile.Spotify
{
    public class SpotifyUserProfile
    {
        [JsonPropertyName("ClientName")]
        public string ClientName { get; set; } = string.Empty;

        [JsonPropertyName("AccessToken")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("ApiBaseUri")]
        public string ApiBaseUri { get; set; } = string.Empty;

        [JsonPropertyName("Endpoint")]
        public string Endpoint { get; set; } = string.Empty;
    }
}