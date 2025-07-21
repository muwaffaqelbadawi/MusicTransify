using System;

namespace MusicTransify.src.Dtos.Transfer
{
    public class TransferTrackDto
    {
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string? Album { get; set; } = string.Empty;
        public string? Duration { get; set; } = string.Empty;
        public string? SourceId { get; set; } = string.Empty;
        public string? ISRC { get; set; } = string.Empty;
    }
}