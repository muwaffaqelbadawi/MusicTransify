using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.src.Models.YouTubeMusic
{
    public class YouTubeMusicData
    {
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public string? Album { get; set; }
        public string? Duration { get; set; }
        public string? SourceId { get; set; }
        public string? ISRC { get; set; }
    }
}