using System;

namespace MusicTransify.src.Contracts.Services.Playlists.Spotify
{
    public interface ISpotifyPlaylistsService
    {
        public Task<T> GetPlaylistAsync<T>();
    }
}