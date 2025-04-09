using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using SpotifyWebAPI_Intro.Services;



namespace SpotifyWebAPI_Intro.Routes
{
    public class AuthRoutes()
    {
        // LoginRoute
        public static void Login(HttpContext context, OptionsService _options)
        {
            // Set webpage content type
            context.Response.ContentType = "text/html";

            // Set Client ID
            string ClientID = _options.SpotifyClientId;

            // Set Response Type
            const string ResponseType = "code";

            // Set the scope value
            const string SCOPE = "user-read-private user-read-email";

            // Set Redirect URI
            string RedirectURI = _options.SpotifyRedirectUri;

            // Set Auth URL
            string AuthURL = _options.SpotifyAuthUrl;

            // Querry Parameters
            var queryParameters = new
            {
                client_id = ClientID,
                response_type = ResponseType,
                scope = SCOPE,
                redirect_uri = RedirectURI,
                show_dialog = true
            };

            var queryString = ToQueryString(queryParameters);

            var auth_url = $"{AuthURL}?{queryString}";
            context.Response.Redirect(auth_url);
        }



        // Callback + Refresh Token



        // Callback Rout
        public static async Task Callback(HttpContext context, OptionsService _options)
        {
            // Set webpage content type
            context.Response.ContentType = "text/html";

            // Check for "error" in the query string
            if (context.Request.Query.ContainsKey("error"))
            {
                // Return the JSON error message
                await context.Response.WriteAsJsonAsync(new { error = context.Request.Query["error"] });

                // Terminate the function
                return;
            }

            // Check if "code" exist in the query string
            if (context.Request.Query.ContainsKey("code"))
            {
                // Check if the Response Type value is not null
                string ResponseType = context.Request.Query["code"].ToString() ?? throw new InvalidOperationException("The 'code' parameter is missing in the query string.");

                // Set the Grant Type
                string GrantType = "authorization_code";

                // Set Redirect URI
                string RedirectURI = _options.SpotifyRedirectUri;

                // Set Client ID
                string ClientID = _options.SpotifyClientId;

                // Set Client Secret
                string ClientSecret = _options.SpotifyClientSecret;

                // Set Token URL
                string TokenURL = _options.SpotifyTokenUrl;

                // Initialize the rquest body
                var RequestBody = new Dictionary<string, string>
                {
                  { "code", ResponseType },
                  { "grant_type", GrantType },
                  { "redirect_url", RedirectURI },
                  { "client_id", ClientID },
                  { "client_secret", ClientSecret }
                };

                // Initiate new http class
                using var client = new HttpClient();

                // Form content
                var FormContent = ToFormUrlEncodedContent(RequestBody);

                // Post Form Url Encoded Content
                var response = await client.PostAsync(TokenURL, FormContent);

                // Handling response error
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error fetching playlists: {response.StatusCode}");
                }

                // Getting result as a response from Spotify server
                var result = await response.Content.ReadAsStringAsync();

                // Deserialize Token Info from result
                var TokenInfo = JsonSerializer.Deserialize<JsonElement>(result);

                // Extract access_token from Token Info if not null
                string access_token = TokenInfo.GetString("access_token") ?? throw new InvalidOperationException("No 'access_token' found");

                // Extract refresh_token from Token Info if not null
                string refresh_token = TokenInfo.GetString("refresh_token") ?? throw new InvalidOperationException("No 'refresh_token' found");

                // Check if expires_in exists in the session and is a valid int
                if (int.TryParse(TokenInfo.GetString("expires_in"), out int int_expires_in))
                {
                    // Handle missing/invalid expiry
                    throw new InvalidOperationException("No 'expires_in' found");
                }

                // Convert int_expires_in to time stamp as (Long) long_expires_in
                long long_expires_in = DateTimeOffset.UtcNow.AddSeconds(int_expires_in).ToUnixTimeSeconds();

                // Store access_token in session
                context.Session.SetString("access_token", access_token);

                // Store refresh_token in session
                context.Session.SetString("refresh_token", refresh_token);

                // Store expires_in in session
                context.Session.SetString("expires_in", long_expires_in.ToString());

                // Redirect to playlists
                context.Response.Redirect("/playlists");

                // Terminate the function
                return;
            }

            else
            {
                // Handle the case where neither "error" nor "code" is present
                await context.Response.WriteAsJsonAsync(new { error = "Invalid request" });

                // Terminate the function
                return;
            }
        }

        // RefreshTokenRoute
        public static async Task RefreshToken(HttpContext context, OptionsService _options)
        {
            // Set webpage content type
            context.Response.ContentType = "text/html";

            // Check if access_token exists in the session and is not null
            if (string.IsNullOrEmpty(context.Session.GetString("access_token")))
            {
                // Redirect back to Spotify login page
                context.Response.Redirect("/login");

                // Terminate the function
                return;
            }

            // Check if expires_in exists in the session and is a valid long
            if (!long.TryParse(context.Session.GetString("expires_in"), out long expires_in))
            {
                // Handle missing/invalid expiry
                throw new InvalidOperationException("No 'expires_in' found");
            }

            // Calculate the current time
            long current_time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Check If the access_token has expired
            if (current_time > expires_in)
            {
                Console.WriteLine("TOKEN EXPIRED. REFRESHING...");

                // Set the grant_type
                string GrantType = "refresh_token";

                // Check if the OldRefreshToken value is not null
                string OldRefreshToken = context.Request.Query["refresh_token"].ToString() ?? throw new InvalidOperationException("The 'refresh_token' parameter is missing in the query string.");

                // Set Client ID
                string ClientID = _options.SpotifyClientId;

                // Set Client Secret
                string ClientSecret = _options.SpotifyClientSecret;

                // Set Token URL
                string TokenURL = _options.SpotifyTokenUrl;

                // Initialize request body
                var RequestBody = new Dictionary<string, string>
                {
                  { "grant_type", GrantType },
                  { "refresh_token", OldRefreshToken },
                  { "client_id", ClientID },
                  { "client_secret", ClientSecret }
                };

                // Initiate new http class
                using var client = new HttpClient();

                // Form content
                var FormContent = ToFormUrlEncodedContent(RequestBody);

                // Post Form Url Encoded Content
                var response = await client.PostAsync(TokenURL, FormContent);

                // Handling response error
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error fetching playlists: {response.StatusCode}");
                }

                // Getting result as a response from Spotify Server
                var result = await response.Content.ReadAsStringAsync();

                // Deserialize Token Info from result
                var TokenInfo = JsonSerializer.Deserialize<JsonElement>(result);

                // Extract access_token from Token Info if not null
                string access_token = TokenInfo.GetString("access_token") ?? throw new InvalidOperationException("No 'access_token' found");

                // Extract refresh_token from Token Info if not null
                string refresh_token = TokenInfo.GetString("refresh_token") ?? throw new InvalidOperationException("No 'refresh_token' found");

                // Check if expires_in exists in the session and is a valid int
                if (int.TryParse(TokenInfo.GetString("expires_in"), out int int_expires_in))
                {
                    // Handle missing/invalid expiry
                    throw new InvalidOperationException("No 'expires_in' found");
                }

                // Convert int_expires_in to time stamp as (Long) long_expires_in
                long long_expires_in = DateTimeOffset.UtcNow.AddSeconds(int_expires_in).ToUnixTimeSeconds();

                // Calculate total expires_in
                long new_expires_in = current_time + long_expires_in;

                // Store access_token in session
                context.Session.SetString("access_token", access_token);

                // Store refresh_token in session
                context.Session.SetString("refresh_token", refresh_token);

                // Update session
                context.Session.SetString("expires_in", new_expires_in.ToString());

                // Redirect to playlists
                context.Response.Redirect("/playlists");

                // Terminate the function
                return;
            }
        }

        // Helper for building query strings
        public static string ToQueryString(object queryParameters)
        {
            return string.Join("&",
            queryParameters.GetType().GetProperties()
            .Select(prop => $"{prop.Name}={Uri.EscapeDataString(prop.GetValue(queryParameters)?.ToString() ?? string.Empty)}"));
        }

        // Helper for creating request body
        public static FormUrlEncodedContent ToFormUrlEncodedContent(Dictionary<string, string> parameters)
        {
            return new FormUrlEncodedContent(parameters);
        }
    }   
}