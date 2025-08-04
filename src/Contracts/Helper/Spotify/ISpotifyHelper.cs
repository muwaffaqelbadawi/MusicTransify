using System;

namespace MusicTransify.src.Contracts.Helper.Spotify
{
    public interface ISpotifyHelper
    {
        public Dictionary<string, string> BuildLoginRequest();
        public Dictionary<string, string> BuildCodeExchangeRequest(string code);
        public Dictionary<string, string> BuildRefreshTokenRequest(string refreshToken);

        // Those will implement an annonymous/lambda function
        string ClientName { get; }
        string AuthUri { get; }
        string TokenUri { get; }
    }
}