using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTransify.src.Models.Spotify
{
    public class SpotifyData
    {
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public string? Album { get; set; }
        public string? Duration { get; set; }
        public string? SourceId { get; set; }
        public string? ISRC { get; set; }
    }
}