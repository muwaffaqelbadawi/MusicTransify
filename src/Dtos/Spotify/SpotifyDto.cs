using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTransify.src.Dtos.Spotify
{
    public class SpotifyDto
    {
        public string? Title { get; set; } = string.Empty;
        public string? Artist { get; set; } = string.Empty;
        public string? Album { get; set; } = string.Empty;
        public string? Duration { get; set; } = string.Empty;
        public string? SourceId { get; set; } = string.Empty;
        public string? ISRC { get; set; } = string.Empty;
    }
}