using System;
using MusicTransify.src.Api.Endpoints.DTOs.Responses.Playlists.YouTubeMusic;

namespace MusicTransify.src.Contracts.Services.Playlists.YouTubeMusic
{
    public interface IYouTubeMusicPlaylistsService
    {
        public Task<YouTubeMusicPlaylistsResponseWrapperDto> GetPlaylistsAsync();
    }
}