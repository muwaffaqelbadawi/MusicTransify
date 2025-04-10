using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using SpotifyWebAPI_Intro.Services;


namespace SpotifyWebAPI_Intro.Routes
{
    // PlaylistsRoute
    public class PlaylistsRoutes()
    {
        public static async Task GetPlaylists(HttpContext context, OptionsService _optionsService)
        {
            // Set webpage content type
            context.Response.ContentType = "text/html";

            // Set APIBase URI
            string APIBaseURL = _optionsService.SpotifyApiBaseUrl;

            // Check if access_token exists in the session and is not null
            if (string.IsNullOrEmpty(context.Session.GetString("access_token")))
            {
                // Redirect back to Spotify login page
                context.Response.Redirect("/login");
                
                // Terminate the function
                return;
            }



            // Set and check expires_in is not null
            string StrExpiresIn = context.Session.GetString("expires_in") ?? throw new InvalidOperationException("No 'expires_in' found");

            // Set ExpiresIn
            long ExpiresIn = long.Parse(StrExpiresIn);

            // Set current time
            long CurrentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Check If the access_token is expired
            if (CurrentTime > ExpiresIn)
            {
                // Console prompt for debugging
                Console.WriteLine("TOKEN EXPIRED. REFRESHING...");

                // Redirect to refresh_token
                context.Response.Redirect("/refresh_token");
               
                // Terminate the function
                return;
            }


            // -----------------------------------------------------------------------------------------

            // Create Autorization String
            string Authorization = $"Bearer {context.Session.GetString("access_token")}";
 
            // ------------------------------------------------------------------------------------------

            //Initiate new http class
            using var client = new HttpClient();

            // Authorization Header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Authorization);

            // Post Form Url Encoded Content
            var response = await client.GetAsync($"{APIBaseURL}me/playlists");

            // Handling response error
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching playlists: {response.StatusCode}");
            }

            // Getting result as a response
            var result = await response.Content.ReadAsStringAsync();

            // Deserialize playlist
            var playlists = JsonSerializer.Deserialize<JsonElement>(result);

            // -----------------------------------------------------------------------------------------

            // Getting the playlists response back
            await context.Response.WriteAsJsonAsync(playlists);
        }
    }
}