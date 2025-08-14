export interface YouTubeMusicPlaylistsResponseWrapperDto {
  items: YouTubeMusicPlaylistsResponseDto[];
}

export interface YouTubeMusicPlaylistsResponseDto {
  kind: string;
  etag: string;
  id: string;
  snippet: YouTubeMusicSnippet;
  ContentDetails: YouTubeMusicContentDetails;
  PageInfo: YouTubeMusicPageInfo;
}

export interface YouTubeMusicContentDetails {
  videoId?: string;
  VideoPublishedAt?: string;
}

export interface YouTubeMusicPageInfo {
  TotalResults: number;
  ResultsPerPage: number;
}

export interface YouTubeMusicSnippet {
  title: string;
  description: string;
  thumbnails: YouTubeMusicThumbnails;
}

export interface YouTubeMusicThumbnails {
  default: YouTubeMusicThumbnailDetail;
  medium: YouTubeMusicThumbnailDetail;
  high: YouTubeMusicThumbnailDetail;
}

export interface YouTubeMusicThumbnailDetail {
  url: string;
  width: number;
  height: number;
}
