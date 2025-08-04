using System;

namespace MusicTransify.src.Contracts.DTOs.Response.Token.YouTubeMusic
{
    public class YouTubeMusicTokenResponse
    {
        public string AccessToken { get; } = string.Empty;
        public string TokenType { get; } = string.Empty;
        public int ExpiresIn { get; }
        public string RefreshToken { get; } = string.Empty;
        public string Scope { get; } = string.Empty;
    }
}