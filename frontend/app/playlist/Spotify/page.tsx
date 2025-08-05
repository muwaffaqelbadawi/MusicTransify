// app/playlist/spotify/page.tsx

"use client";

import { useEffect, useState } from "react";

interface SpotifyPlaylist {
  Id: number;
  Name: string;
  tracksCount: number;
}

export default function SpotifyPlaylists() {
  const [playlists, setPlaylists] = useState<SpotifyPlaylist[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await fetch(
          "https://localhost:5543/spotify/playlist"
        );
        const data = await response.json();
        setPlaylists(data);
      } catch (error) {
        console.error("Error fetching products:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchProducts();
  }, []);

  if (loading) return <p>Loading...</p>;

  return (
    <div>
      <h1>Products List</h1>
      <ul>
        {products.map((product) => (
          <li key={product.Id}>
            {product.Name} - ${product.Price}
          </li>
        ))}
      </ul>
    </div>
  );
}
