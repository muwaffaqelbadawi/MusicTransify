using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTransify.src.Models.Transfer
{
    public enum PlatformType
    {
        Spotify,
        YouTubeMusic
        // Add other platforms as needed
    }

    public class TransferRequest
    {
        public string? SourcePlaylistId { get; set; }
        public PlatformType SourcePlatform { get; set; }
        public PlatformType DestinationPlatform { get; set; }
    }
}