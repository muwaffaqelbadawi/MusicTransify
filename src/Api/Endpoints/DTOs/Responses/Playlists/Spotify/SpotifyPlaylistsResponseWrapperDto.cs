using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Api.Endpoints.DTOs.Responses.Playlists.Spotify
{
    public class SpotifyPlaylistsResponseWrapperDto
    {
        [JsonPropertyName("items")]
        public List<SpotifyPlaylistsResponseDto>? Items { get; set; }
    }
}