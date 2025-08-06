// types/YouTubeMusic.ts (create this file for cleaner code separation)

export interface YouTubeMusicPlaylistsResponseWrapper {
  items: YouTubeMusicPlaylistResponse[];
}

export interface YouTubeMusicPlaylistResponse {
  id: string;
  name: string;
  images: YouTubeMusicImage[];
  owner: YouTubeMusicOwner;
  public: boolean;
  tracks: YouTubeMusicTrackLink;
  type: string;
  uri: string;
}

export interface YouTubeMusicImage {
  url: string;
  height?: number;
  width?: number;
}

export interface YouTubeMusicOwner {
  display_name: string;
}

export interface YouTubeMusicTrackLink {
  href: string;
  total: number;
}
