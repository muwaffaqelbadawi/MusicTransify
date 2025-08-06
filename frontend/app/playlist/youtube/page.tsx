// app/playlist/youtube/page.tsx

"use client";

import { useEffect, useState } from "react";

import {
  YouTubeMusicPlaylistResponse,
  YouTubeMusicPlaylistsResponseWrapper
} from "@/types/youtube"; // Adjust path as needed

export default function YouTubeMusicPlaylists() {
  const [playlists, setPlaylists] = useState<YouTubeMusicPlaylistResponse[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchlaylists = async () => {
      try {
        const response = await fetch(
          "http://localhost:5543/api/youtube/playlist"
        );
        const data = await response.json();
        setPlaylists(data);
      } catch (error) {
        console.error("Error fetching playlists:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchlaylists();
  }, []);

  if (loading) return <p>Loading YouTubeMusic playlists...</p>;

  return (
    <div>
      <h1>🎧 YouTubeMusic Playlists</h1>
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
