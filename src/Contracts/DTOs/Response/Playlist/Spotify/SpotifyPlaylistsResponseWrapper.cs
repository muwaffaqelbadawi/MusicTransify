using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Contracts.DTOs.Response.Playlist.Spotify
{
    public class SpotifyPlaylistsResponseWrapper
    {
        [JsonPropertyName("items")]
        public List<SpotifyPlaylistResponse> Items { get; set; } = new();
    }
}