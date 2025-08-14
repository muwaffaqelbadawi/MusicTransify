using System;

namespace MusicTransify.src.Contracts.Utilities.YouTubeMusic
{
    public interface IYouTubeMusicHelper
    {
        public Dictionary<string, string> BuildLoginRequest();
        public Dictionary<string, string> BuildCodeExchangeRequest(string code);
        public Dictionary<string, string> BuildRefreshTokenRequest(string refreshToken);
    }
}