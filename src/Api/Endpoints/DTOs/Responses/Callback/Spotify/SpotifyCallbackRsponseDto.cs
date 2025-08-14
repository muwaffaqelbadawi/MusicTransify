using System;
using Microsoft.AspNetCore.Mvc;

namespace MusicTransify.src.Api.Endpoints.DTOs.Responses.Callback.Spotify
{
    public class SpotifyCallbackResponseDto
    {
        [FromQuery(Name = "code")]
        public string? Code { get; set; }
        [FromQuery(Name = "state")]
        public string? State { get; set; }
        [FromQuery(Name = "error")]
        public string? Error { get; set; }
    }
}