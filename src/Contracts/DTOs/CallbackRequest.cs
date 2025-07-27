using System;

namespace MusicTransify.src.Contracts.DTOs
{
    public class CallbackRequest
    {
        public string? Code { get; set; }
        public string? State { get; set; }
        public string? Scope { get; set; } // optional, for YouTube API
        public string? Error { get; set; } // optional, for error handling
    }
}