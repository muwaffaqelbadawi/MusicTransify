using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Api.Spotify.Playlists.Responses
{
    public class SpotifyPlaylistsResponseWrapperDto
    {
        [JsonPropertyName("items")]
        public List<SpotifyPlaylistsResponseDto>? Items { get; set; }
    }
}