"use client";

import {
  SpotifyPlaylistsResponseDto,
  SpotifyPlaylistsResponseWrapperDto,
} from "@/types/spotify";
import { useEffect, useState } from "react";

export default function SpotifyPlaylists() {
  const [playlists, setPlaylists] = useState<SpotifyPlaylistsResponseDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchPlaylists = async () => {
      try {
        const response = await fetch(
          "http://localhost:5543/api/spotify/playlists",
          {
            method: "GET",
            credentials: "include",
          }
        );
        
        const data: SpotifyPlaylistsResponseWrapperDto = await response.json();
        setPlaylists(data.items);

      } catch (error) {
        console.error("Error fetching playlists:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchPlaylists();
  }, []);

  if (loading) return <p>Loading Spotify playlists...</p>;

  return (
    <div>
      <h1>🎧 Spotify Playlists</h1>
      <ul>
        {playlists.map((playlist) => (
          <li key={playlist.id}>
            <img
              src={playlist.images[0]?.url}
              alt={`${playlist.name} cover`}
              style={{ width: "100px", height: "100px", objectFit: "cover" }}
            />
            <div>
              <strong>{playlist.name}</strong>
              <br />
              👤 Owner: {playlist.owner.display_name}
              <br />
              🎵 Tracks: {playlist.tracks.total}
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}
