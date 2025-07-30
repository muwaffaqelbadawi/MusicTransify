using System;

namespace MusicTransify.src.Contracts.Services.Playlist.Spotify
{
    public interface ISpotifyPlaylistService
    {
        public Task<T> GetPlaylistAsync<T>(string id);
    }
}