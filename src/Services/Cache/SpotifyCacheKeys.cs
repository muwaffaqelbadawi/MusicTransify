using System;

namespace MusicTransify.src.Services.Cache
{
    public static class SpotifyCacheKeys
    {
        public static string UserPlaylists(string userId) => $"Spotify:User:{userId}:Playlists";
        public static string PlaylistTracks(string playlistId) => $"Spotify:Playlist:{playlistId}:Tracks";
    }

}