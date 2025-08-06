export default function Home() {
  return (
    <main>
      <h1>Welcome to MusicTransify 🎶</h1>
      <p>This is your Next.js homepage connected to your .NET backend</p>
      <div style={{ display: "flex", gap: "10px", marginTop: "20px" }}>
        <a
          href="http://localhost:5543/api/spotify/login"
          style={{ padding: "10px", background: "green", color: "white" }}
        >
          Log in with Spotify
        </a>
        <a
          href="http://localhost:5543/api/youtube/login"
          style={{ padding: "10px", background: "red", color: "white" }}
        >
          Log in with YouTube
        </a>
      </div>
    </main>
  );
}
