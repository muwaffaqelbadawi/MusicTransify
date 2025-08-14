using System;

namespace MusicTransify.src.Contracts.Utilities.YouTubeMusic
{
    public interface IYouTubeMusicPlaylistsHelper
    {
        public HttpRequestMessage BuildPlaylistRequest();
    }
}