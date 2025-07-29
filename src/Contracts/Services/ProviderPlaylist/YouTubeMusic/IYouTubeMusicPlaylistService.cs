using System;

namespace MusicTransify.src.Contracts.Services.ProviderPlaylist.YouTubeMusic
{
    public interface IYouTubeMusicPlaylistService
    {
        public Task<T> GetPlaylistAsync<T>(string id);
    }
}