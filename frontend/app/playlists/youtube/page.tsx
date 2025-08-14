"use client";

import { useEffect, useState } from "react";

import {
  YouTubeMusicPlaylistsResponseDto,
  YouTubeMusicPlaylistsResponseWrapperDto,
} from "@/types/youtube";

export default function YouTubeMusicPlaylists() {
  const [playlists, setPlaylists] = useState<YouTubeMusicPlaylistsResponseDto[]>(
    []
  );
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchPlaylists = async () => {
      try {
        const response = await fetch(
          "http://localhost:5543/api/youtube/playlists",
          {
            method: "GET",
            credentials: "include",
          }
        );

        const data: YouTubeMusicPlaylistsResponseWrapperDto =
          await response.json();
        setPlaylists(data.items);
      } catch (error) {
        console.error("Error fetching playlists:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchPlaylists();
  }, []);

  if (loading) return <p>Loading YouTubeMusic playlists...</p>;

  return (
    <div>
      <h1>🎧 YouTubeMusic Playlists</h1>
      <ul style={{ listStyle: "none", padding: 0 }}>
        {playlists.map((playlist) => (
          <li key={playlist.id} style={{ marginBottom: "1.5rem" }}>
            <div style={{ display: "flex", alignItems: "center", gap: "1rem" }}>
              <img
                src={
                  playlist.snippet?.thumbnails?.medium?.url ||
                  playlist.snippet?.thumbnails?.default?.url
                }
                alt={playlist.snippet.title}
                width={playlist.snippet.thumbnails.medium?.width || 120}
                height={playlist.snippet.thumbnails.medium?.height || 90}
                style={{ borderRadius: 8, objectFit: "cover" }}
              />
              <div>
                <a
                  href={`https://music.youtube.com/playlist?list=${playlist.id}`}
                  target="_blank"
                  rel="noopener noreferrer"
                  style={{
                    fontSize: "1.1rem",
                    fontWeight: "bold",
                    color: "#1db954",
                    textDecoration: "none",
                  }}
                >
                  {playlist.snippet.title}
                </a>
                <p style={{ marginTop: 4, fontSize: "0.9rem", color: "#666" }}>
                  {playlist.snippet.description || "No description available."}
                </p>
                <small style={{ color: "#999" }}>
                  Published:{" "}
                  {playlist.ContentDetails.VideoPublishedAt
                    ? new Date(
                        playlist.ContentDetails.VideoPublishedAt
                      ).toLocaleDateString()
                    : "Unknown"}
                </small>
              </div>
            </div>
            <br />
            <br />
          </li>
        ))}
      </ul>
    </div>
  );
}
