using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Api.YouTubeMusic.Playlists.Responses
{
    public class YouTubeMusicPlaylistsResponseWrapperDto
    {
        [JsonPropertyName("items")]
        public List<YouTubeMusicPlaylistsResponseDto>? Items { get; set; }
    }
}