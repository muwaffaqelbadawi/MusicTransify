using System;
using System.Linq;
using System.Threading.Tasks;
using SpotifyWebAPI_Intro.src.Dtos.Spotify;
using SpotifyWebAPI_Intro.src.Models.Spotify;

namespace SpotifyWebAPI_Intro.src.Mappers.Spotify
{
    public class SpotifyMapper
    {
        public SpotifyDto ToSpotifyDto(SpotifyData spotify)
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