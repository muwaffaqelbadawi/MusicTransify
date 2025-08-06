// app/layout.tsx
import React from "react";
import "../styles/globals.css"; // Optional if you have global CSS

export const metadata = {
  title: "MusicTransify",
  description: "Transfer your music seamlessly across platforms",
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
