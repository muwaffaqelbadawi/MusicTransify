using System;
using Microsoft.AspNetCore.Mvc;

namespace MusicTransify.src.Contracts.DTOs.Request.Shared
{
    public class CallbackRequest
    {
        // For receiving query params from Http query
        [FromQuery(Name = "code")]
        public string? Code { get; set; } = string.Empty;
        [FromQuery(Name = "state")]
        public string? State { get; set; } = string.Empty;
        [FromQuery(Name = "scope")]
        public string? Scope { get; set; } = string.Empty; // optional, for YouTube API
        [FromQuery(Name = "error")]
        public string? Error { get; set; } = string.Empty; // optional, for error handling
    }
}