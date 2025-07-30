using System;

namespace MusicTransify.src.Contracts.Services.Playlist.YouTubeMusic
{
    public interface IYouTubeMusicPlaylistService
    {
        public Task<T> GetPlaylistAsync<T>(string id);
    }
}