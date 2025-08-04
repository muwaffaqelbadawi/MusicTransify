using System;

namespace MusicTransify.src.Contracts.Services.Playlist.Spotify
{
    public interface ISpotifyPlaylistService
    {
        public Task<T> GetPlaylistAsync<T>();
        // public Task<T> GetPlaylistWithIdAsync<T>(string id);
    }
}