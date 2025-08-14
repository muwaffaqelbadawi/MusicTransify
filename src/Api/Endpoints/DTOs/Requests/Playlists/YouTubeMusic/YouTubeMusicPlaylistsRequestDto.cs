using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using MusicTransify.src.Contracts.Mapper;

namespace MusicTransify.src.Api.Endpoints.Dtos.Requests.Playlists.YouTubeMusic
{
    public class YouTubeMusicPlaylistsRequestDto : IMappable
    {
        [JsonPropertyName("PlaylistParameters")]
        public YouTubeMusicPlaylistParameters PlaylistParameters { get; set; } = new();

        public Dictionary<string, string> ToMap() => PlaylistParameters.ToMap();
    }

    public class YouTubeMusicPlaylistParameters : IMappable
    {
        [JsonPropertyName("part")]
        public string? Part { get; set; }

        [JsonPropertyName("mine")]
        public string? Mine { get; set; }
        
        public Dictionary<string, string> ToMap() => new()
        {
            ["part"] = Part ?? string.Empty,
            ["mine"] = Mine ?? string.Empty
        };
    }
}