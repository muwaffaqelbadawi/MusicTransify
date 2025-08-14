using System;
using System.Text.Json.Serialization;

namespace MusicTransify.src.Api.YouTubeMusic.Playlists.Responses
{
    public class YouTubeMusicPlaylistsResponseDto
    {
        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("etag")]
        public string? ETag { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("snippet")]
        public YouTubeMusicSnippet? Snippet { get; set; }

        [JsonPropertyName("contentDetails")]
        public YouTubeMusicContentDetails? ContentDetails { get; set; }

        [JsonPropertyName("pageInfo")]
        public YouTubeMusicPageInfo? PageInfo { get; set; }
    }

    public class YouTubeMusicSnippet
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("thumbnails")]
        public YouTubeMusicThumbnails? Thumbnails { get; set; }
    }

    public class YouTubeMusicThumbnails
    {
        [JsonPropertyName("default")]
        public YouTubeMusicThumbnailDetail? Default { get; set; }

        [JsonPropertyName("medium")]
        public YouTubeMusicThumbnailDetail? Medium { get; set; }

        [JsonPropertyName("high")]
        public YouTubeMusicThumbnailDetail? High { get; set; }
    }

    public class YouTubeMusicThumbnailDetail
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }
    }
    public class YouTubeMusicContentDetails
    {
        [JsonPropertyName("videoId")]
        public string? VideoId { get; set; }

        [JsonPropertyName("videoPublishedAt")]
        public DateTimeOffset? VideoPublishedAt { get; set; }
    }

    public class YouTubeMusicPageInfo
    {
        [JsonPropertyName("totalResults")]
        public int? TotalResults { get; set; }
        [JsonPropertyName("resultsPerPage")]
        public int? ResultsPerPage { get; set; }
    }
}