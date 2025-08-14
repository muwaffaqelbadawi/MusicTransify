using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Api.Endpoints.DTOs.Responses.Playlists.YouTubeMusic
{
    public class YouTubeMusicPlaylistsResponseWrapperDto
    {
        [JsonPropertyName("items")]
        public List<YouTubeMusicPlaylistsResponseDto>? Items { get; set; }
    }
}