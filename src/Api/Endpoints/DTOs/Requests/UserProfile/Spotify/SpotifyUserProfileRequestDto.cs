using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Api.Endpoints.DTOs.Requests.UserProfile.Spotify
{
    public class SpotifyUserProfileRequestDto
    {
        [JsonPropertyName("ClientName")]
        public string? ClientName { get; set; }

        [JsonPropertyName("AccessToken")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("ApiBaseUri")]
        public string? ApiBaseUri { get; set; }

        [JsonPropertyName("Endpoint")]
        public string? Endpoint { get; set; }
    }
}