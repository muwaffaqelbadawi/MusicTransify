using System;

namespace MusicTransify.src.Contracts.Helper.YouTubeMusic
{
    public interface IYouTubeMusicHelper
    {
        public Dictionary<string, string> BuildLoginRequest();
        public Dictionary<string, string> BuildCodeExchangeRequest(string code);
        public Dictionary<string, string> BuildRefreshTokenRequest(string refreshToken);
        string ClientName { get; }
        string AuthUri { get; }
        string TokenUri { get; }
    }
}