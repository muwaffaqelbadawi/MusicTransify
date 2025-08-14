using System;
using MusicTransify.src.Api.YouTubeMusic.Playlists.Responses;

namespace MusicTransify.src.Contracts.Services.Playlists.YouTubeMusic
{
    public interface IYouTubeMusicPlaylistsService
    {
        public Task<YouTubeMusicPlaylistsResponseWrapperDto> GetPlaylistsAsync();
    }
}