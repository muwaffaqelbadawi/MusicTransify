export interface SpotifyPlaylistsResponseWrapperDto {
  items: SpotifyPlaylistsResponseDto[];
}

export interface SpotifyPlaylistsResponseDto {
  id: string;
  name: string;
  images: SpotifyImage[];
  owner: SpotifyOwner;
  public: boolean;
  tracks: SpotifyTrackLink;
  type: string;
  uri: string;
}

export interface SpotifyImage {
  url: string;
  height?: number;
  width?: number;
}

export interface SpotifyOwner {
  display_name: string;
}

export interface SpotifyTrackLink {
  href: string;
  total: number;
}