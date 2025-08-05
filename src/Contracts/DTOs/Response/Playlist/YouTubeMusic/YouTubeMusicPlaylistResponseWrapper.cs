using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Contracts.DTOs.Response.Playlist.YouTubeMusic
{
    public class YouTubeMusicPlaylistResponseWrapper
    {
        [JsonPropertyName("items")]
        public List<YouTubeMusicPlaylistResponse> Items { get; set; } = new();
    }
}