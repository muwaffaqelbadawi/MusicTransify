using System;
using Microsoft.AspNetCore.Mvc;

namespace MusicTransify.src.Api.YouTubeMusic.Callback.Responses
{
    public class YouTubeMusicCallbackResponseDto
    {
        [FromQuery(Name = "code")]
        public string? Code { get; set; }
        [FromQuery(Name = "state")]
        public string? State { get; set; }
        [FromQuery(Name = "scope")]
        public string? Scope { get; set; }
        [FromQuery(Name = "error")]
        public string? Error { get; set; }
    }
}