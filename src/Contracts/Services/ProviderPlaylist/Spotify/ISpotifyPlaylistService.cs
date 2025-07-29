using System;

namespace MusicTransify.src.Contracts.Services.ProviderPlaylist.Spotify
{
    public interface ISpotifyPlaylistService
    {
        public Task<T> GetPlaylistAsync<T>(string id);
    }
}