// using System;
// using System.Net.Http;
// using System.Text;
// using System.Text.Json;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.Extensions.DependencyInjection;
// using SpotifyAPI.Web;
// using SpotifyAPI.Web.Auth;

// class Program
// {
//     // Add your Spotify Client ID here
//     private const string CLIENT_ID = "YOUR_CLIENT_ID";

//     // Add your Spotify Client Secret here
//     private const string CLIENT_SECRET = "YOUR_CLIENT";
//     private const string REDIRECT_URI = "http://localhost:5543/callback";
//     private const string AUTH_URL = "https://accounts.spotify.com/authorize";
//     private const string TOKEN_URL = "https://accounts.spotify.com/api/token";
//     private const string API_BASE_URL = "https://api.spotify.com/v1/";

//     static async Task Main(string[] args)
//     {
//         // Make sure "http://localhost:5543/callback" is in your application's redirect URIs!
//         var loginRequest = new LoginRequest(
//             new Uri(REDIRECT_URI),
//             CLIENT_ID,
//             LoginRequest.ResponseType.Code
//         )
//         {
//             Scope = new[] { Scopes.PlaylistReadPrivate, Scopes.PlaylistReadCollaborative }
//         };
//         var uri = loginRequest.ToUri();
//         Console.WriteLine("Please visit this URL and authorize the application:");
//         Console.WriteLine(uri);

//         // Set up a simple web server to handle the callback
//         var builder = WebApplication.CreateBuilder(args);
//         var app = builder.Build();

//         app.MapGet("/callback", async (HttpContext context) =>
//         {
//             var code = context.Request.Query["code"].ToString();

//             if (string.IsNullOrEmpty(code))
//             {
//                 await context.Response.WriteAsync("Authorization code not found in the query string.");
//                 return;
//             }

//             var response = await new OAuthClient().RequestToken(
//                 new AuthorizationCodeTokenRequest(CLIENT_ID, CLIENT_SECRET, code, new Uri(REDIRECT_URI))
//             );

//             var spotify = new SpotifyClient(response.AccessToken);


//             // Console.WriteLine($"Access Token: {response.AccessToken}");
//             // Console.WriteLine($"Refresh Token: {response.RefreshToken}");
//             // Console.WriteLine($"Expires In: {response.ExpiresIn} seconds");

//             // Example of using the access token to make an API request
//             var playlists = await spotify.Playlists.CurrentUsers();
//             if (playlists.Items != null)
//             {
//                 await context.Response.WriteAsync($"You have {playlists.Items.Count} playlists.");
//             }
//             else
//             {
//                 await context.Response.WriteAsync("No playlists found.");
//             }
//         });

//         await app.RunAsync("http://localhost:5543");
//     }
// }