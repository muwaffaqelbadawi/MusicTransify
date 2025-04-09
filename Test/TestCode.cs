/*

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;



namespace SpotifyWebAPI_Intro.Test
{
    public class TestCode
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var app = builder.Build();
            app.UseSession();
            app.UseMiddleware<AuthenticationMiddleware>();

            app.MapGet("/", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("Welcome to Spotify App <a href='/login'>Login with Spotify</a>");
            });

            app.MapGet("/login", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html";
                const string SCOPE = "user-read-private user-read-email";
                const string RESPONSE_TYPE = "code";

                var queryParameters = new
                {
                    client_id = CLIENT_ID,
                    response_type = RESPONSE_TYPE,
                    scope = SCOPE,
                    redirect_uri = REDIRECT_URI,
                    show_dialog = true
                };

                var queryString = string.Join("&",
                    queryParameters.GetType().GetProperties().Select(prop => $"{prop.Name}={prop.GetValue(queryParameters)}"));

                var authUrl = $"{AUTH_URL}?{queryString}";
                context.Response.Redirect(authUrl);

                await Task.CompletedTask;
            });

            app.MapGet("/callback", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html";

                if (context.Request.Query.ContainsKey("error"))
                {
                    await context.Response.WriteAsJsonAsync(new { error = context.Request.Query["error"] });
                    return;
                }

                if (context.Request.Query.ContainsKey("code"))
                {
                    string code = context.Request.Query["code"].ToString() ?? throw new InvalidOperationException("The 'code' parameter is missing in the query string.");

                    var reqBody = new Dictionary<string, string>
                    {
                    { "code",  code },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", REDIRECT_URI },
                    { "client_id", CLIENT_ID },
                    { "client_secret", CLIENT_SECRET }
                    };

                    var response = await PostFormUrlEncodedContent(TOKEN_URL, reqBody);

                    var tokenInfo = JsonSerializer.Deserialize<JsonElement>(response);

                    string accessToken = tokenInfo.GetProperty("access_token").ToString();
                    string refreshToken = tokenInfo.GetProperty("refresh_token").ToString();
                    double expiresIn = tokenInfo.GetProperty("expires_in").GetDouble();
                    long newExpiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresIn).ToUnixTimeSeconds();

                    context.Session.SetString("access_token", accessToken);
                    context.Session.SetString("refresh_token", refreshToken);
                    context.Session.SetString("expires_at", newExpiresAt.ToString());

                    context.Response.Redirect("/playlists");
                }
                else
                {
                    await context.Response.WriteAsJsonAsync(new { error = "Invalid request" });
                }
            });

            app.MapGet("/playlists", async (HttpContext context) =>
            {
                string accessToken = context.Session.GetString("access_token") ?? throw new InvalidOperationException("No access_token found");

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{API_BASE_URL}me/playlists", content);
                var result = await response.Content.ReadAsStringAsync();

                var playlists = JsonSerializer.Deserialize<JsonElement>(result);
                await context.Response.WriteAsJsonAsync(playlists);
            });

            app.MapGet("/refresh_token", async (HttpContext context) =>
            {
                string refreshToken = context.Session.GetString("refresh_token") ?? throw new InvalidOperationException("No refresh_token found");
                var reqBody = new Dictionary<string, string>
                {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
                { "client_id", CLIENT_ID },
                { "client_secret", CLIENT_SECRET }
                };

                var response = await PostFormUrlEncodedContent(TOKEN_URL, reqBody);
                var newTokenInfo = JsonSerializer.Deserialize<JsonElement>(response);
                string accessToken = newTokenInfo.GetProperty("access_token").ToString();
                double expiresIn = newTokenInfo.GetProperty("expires_in").GetDouble();
                long newExpiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresIn).ToUnixTimeSeconds();

                context.Session.SetString("access_token", accessToken);
                context.Session.SetString("expires_at", newExpiresAt.ToString());

                context.Response.Redirect("/playlists");
            });

            await app.RunAsync("http://localhost:5543");
        }

        private static async Task<string> PostFormUrlEncodedContent(string url, Dictionary<string, string> reqBody)
        {
            using var client = new HttpClient();
            var response = await client.PostAsync(url, new FormUrlEncodedContent(reqBody));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static bool IsTokenExpired(HttpContext context)
        {
            if (!long.TryParse(context.Session.GetString("expires_at"), out long expiresAt))
            {
                throw new InvalidOperationException("No expires_at found");
            }

            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return currentTime > expiresAt;
        }
    }
}

*/