import React from "react";
import "../styles/globals.css";

export const metadata = {
  title: "MusicTransify",
  description:
    "MusicTransify is an open-source web app designed to sync your favorite playlists between Spotify and YouTube Music",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body>
        {/* You can add a navbar here if needed */}
        {children}
      </body>
    </html>
  );
}
