using System;

namespace MusicTransify.src.Contracts.Helper.Spotify
{
    public interface ISpotifyPlaylistHelper
    {
        public HttpRequestMessage BuildPlaylistRequest();
        public HttpRequestMessage BuildPlaylistRequestWithId(string Id);
        string ClientName { get; }
    }
}