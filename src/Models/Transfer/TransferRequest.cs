using System;

namespace MusicTransify.src.Models.Transfer
{
    public enum PlatformType
    {
        Spotify,
        YouTubeMusic
    }

    public class TransferRequest
    {
        public string? SourcePlaylistId { get; set; }
        public PlatformType SourcePlatform { get; set; }
        public PlatformType DestinationPlatform { get; set; }
    }
}