using System;

namespace MusicTransify.src.Contracts.Helper.YouTubeMusic
{
    public interface IYouTubeMusicPlaylistHelper
    {
        public HttpRequestMessage BuildPlaylistRequest();
        public HttpRequestMessage BuildPlaylistRequestWithId(string playlistId);

        // This is will implement an annonymous/lambda function to return the client name
        string ClientName { get; }
    }
}