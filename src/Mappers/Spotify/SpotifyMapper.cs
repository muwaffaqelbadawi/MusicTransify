using System;
using MusicTransify.src.Dtos.Spotify;
using MusicTransify.src.Models.Spotify;

namespace MusicTransify.src.Mappers.Spotify
{
    public class SpotifyMapper
    {
        public SpotifyDto ToSpotifyDto(Data spotify)
        {
            return new SpotifyDto
            {
                Title = spotify.Title,
                Artist = spotify.Artist,
                Album = spotify.Album,
                Duration = spotify.Duration,
                SourceId = spotify.SourceId,
                ISRC = spotify.ISRC,
            };
        }
    }
}