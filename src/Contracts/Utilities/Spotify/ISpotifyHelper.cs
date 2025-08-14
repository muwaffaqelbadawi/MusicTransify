using System;

namespace MusicTransify.src.Contracts.Utilities.Spotify
{
    public interface ISpotifyHelper
    {
        public Dictionary<string, string> BuildLoginRequest();
        public Dictionary<string, string> BuildCodeExchangeRequest(string code);
        public Dictionary<string, string> BuildRefreshTokenRequest(string refreshToken);
    }
}