using System;
using MusicTransify.src.Api.YouTubeMusic.Playlists.Responses;

namespace MusicTransify.src.Utilities.DTO.YouTubeMusic
{
    public static class YouTubeMusicPlaylistsResponseHelper
    {
        public static (YouTubeMusicSnippet Snippet, bool HadMissingFields) FillSnippets(YouTubeMusicSnippet? snippet)
        {
            bool HadMissingFields = false; // This is a checking flag

            if (snippet == null)
            {
                snippet = new YouTubeMusicSnippet();
                HadMissingFields = true;
            }

            if (snippet.Title == null)
            {
                snippet.Title = "unknown";
                HadMissingFields = true;
            }

            if (snippet.Description == null)
            {
                snippet.Description = "unknown";
                HadMissingFields = true;
            }

            snippet.Thumbnails = FillThumbnails(snippet.Thumbnails);
            return (snippet, HadMissingFields);
        }

        public static YouTubeMusicThumbnails FillThumbnails(YouTubeMusicThumbnails? thumbnails)
        {

            if (thumbnails == null) thumbnails = new YouTubeMusicThumbnails();

            thumbnails.Default ??= FillThumbs(thumbnails.Default);
            thumbnails.Medium ??= FillThumbs(thumbnails.Medium);
            thumbnails.High ??= FillThumbs(thumbnails.High);

            return thumbnails;
        }

        public static YouTubeMusicThumbnailDetail FillThumbs(YouTubeMusicThumbnailDetail? thumb)
        {

            if (thumb == null) thumb = new YouTubeMusicThumbnailDetail();

            thumb.Url ??= "unknown";
            thumb.Width ??= 0;
            thumb.Height ??= 0;

            return thumb;
        }

        public static YouTubeMusicContentDetails FillDePageInfo(YouTubeMusicContentDetails? contentDetails)
        {
            if (contentDetails == null) contentDetails = new YouTubeMusicContentDetails();

            contentDetails.VideoId ??= "unknown";
            contentDetails.VideoPublishedAt ??= DateTime.MinValue;

            return contentDetails;
        }

        public static YouTubeMusicPageInfo FillDePageInfo(YouTubeMusicPageInfo? pageInfo)
        {
            if (pageInfo == null) pageInfo = new YouTubeMusicPageInfo();

            pageInfo.TotalResults ??= 0;
            pageInfo.ResultsPerPage ??= 0;

            return pageInfo;
        }
    }
}
