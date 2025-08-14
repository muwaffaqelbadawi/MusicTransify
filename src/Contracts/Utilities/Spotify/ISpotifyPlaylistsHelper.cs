using System;

namespace MusicTransify.src.Contracts.Utilities.Spotify
{
    public interface ISpotifyPlaylistsHelper
    {
        public HttpRequestMessage BuildPlaylistRequest();
    }
}