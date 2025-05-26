using System;
using System.Linq;
using System.Threading.Tasks;
using SpotifyWebAPI_Intro.src.Dtos.YouTubeMusic;
using SpotifyWebAPI_Intro.src.Models.YouTubeMusic;

namespace SpotifyWebAPI_Intro.src.Mappers.YouTubeMusic
{
    public class YouTubeMusicMapper
    {
        public YouTubeMusicDto ToSpotifyDto(YouTubeMusicData youTubeMusic)
        {
            return new YouTubeMusicDto
            {
                Title = youTubeMusic.Title,
                Artist = youTubeMusic.Artist,
                Album = youTubeMusic.Album,
                Duration = youTubeMusic.Duration,
                SourceId = youTubeMusic.SourceId,
                ISRC = youTubeMusic.ISRC,
            };
        }

    }
}