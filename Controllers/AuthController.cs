using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using SpotifyWebAPI_Intro.Services;
using SpotifyWebAPI_Intro.utilities;

namespace SpotifyWebAPI_Intro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly OptionsService _optionsService;
        private readonly SessionService _sessionService;
        private readonly HttpService _httpService;
        private readonly AuthHelper _authHelper;

        public AuthController(IHttpContextAccessor httpContextAccessor, OptionsService optionsService, SessionService sessionService, HttpService httpService, AuthHelper authHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _optionsService = optionsService;
            _sessionService = sessionService;
            _httpService = httpService;
            _authHelper = authHelper;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            string ClientID = _optionsService.SpotifyClientId;

            // Set Response Type
            const string ResponseType = "code";

            // Set the scope value
            const string SCOPE = "user-read-private user-read-email";

            // Set Redirect URI
            string RedirectURI = _optionsService.SpotifyRedirectUri;

            // Set Auth URL
            string AuthURL = _optionsService.SpotifyAuthUrl;

            // Querry Parameters
            var queryParameters = new Dictionary<string, string>
            {
                { "client_id", ClientID },
                { "response_type", ResponseType },
                { "scope", SCOPE },
                { "redirect_uri", RedirectURI },
                { "show_dialog", "true" }
            };

            // Build the query string from the parameters
            var queryString = _authHelper.ToQueryString(queryParameters);

            // Form the authorization URL
            var auth_url = $"{AuthURL}?{queryString}";

            // Redirect to the authorization URL
            Redirect(auth_url);

            // Successful redirection to auth_url
            return Ok(auth_url);
        }

        [HttpGet("Callback")]
        public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string error)
        {
            // Check if "error" exists in the query string and not null
            if (!string.IsNullOrEmpty(error))
            {
                // Return the JSON error message if not exists
                return BadRequest(new { error });
            }

            // Set the Grant Type
            string GrantType = "authorization_code";

            // Set Redirect URI
            string RedirectURI = _optionsService.SpotifyRedirectUri;

            // Set Client ID
            string ClientID = _optionsService.SpotifyClientId;

            // Set Client Secret
            string ClientSecret = _optionsService.SpotifyClientSecret;

            // Set Token URL
            string TokenURL = _optionsService.SpotifyTokenUrl;

            // Build the rquest body
            var RequestBody = new Dictionary<string, string>
            {
                { "code", code },
                { "grant_type", GrantType },
                { "redirect_url", RedirectURI },
                { "client_id", ClientID },
                { "client_secret", ClientSecret }
            };

            // Set Token Info
            var TokenInfo = await _httpService.PostFormUrlEncodedContentAsync(TokenURL, RequestBody);

            // Check existence of token assets
            var Assets = _sessionService.CheckAssets(TokenInfo);

            // Calculate token expiration date
            string ExpiresIn = _sessionService.CalculateTokenExpirationDate(Assets.ExpiresIn);

            // Store token assets in session
            _sessionService.StoreAssetes(Assets.AccessToken, Assets.RefreshToken, ExpiresIn);

            // Redirect back to playlists
            Redirect("/playlists");

            // Successfuly redirect to playlists
            return Ok("Redirect to playlists");
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var AccessToken = _sessionService.RevealAssete("AccessToken");
            var OldExpiresIn = _sessionService.RevealAssete("ExpiresIn");

            // Set and Check if access_token exists in the session and is not null
            if (string.IsNullOrEmpty(AccessToken))
            {
                // The access token is not exist
                Redirect("/login");

                // Redirect back to Spotify login page
                return BadRequest("Redirect to login route");
            }

            // Check If the access_token is expired
            if (_authHelper.IsExpired(OldExpiresIn))
            {
                // Console prompt for debugging
                Console.WriteLine("TOKEN EXPIRED. REFRESHING...");

                // Set the grant_type
                string GrantType = "refresh_token";

                // Check refresh_token value exists in query string and is not null
                string OldRefreshToken = _httpContextAccessor.HttpContext?.Request.Query["refresh_token"].ToString() ?? throw new InvalidOperationException("The 'refresh_token' parameter is missing in the query string.");

                // Set Client ID
                string ClientID = _optionsService.SpotifyClientId;

                // Set Client Secret
                string ClientSecret = _optionsService.SpotifyClientSecret;

                // Set Token URL
                string TokenURL = _optionsService.SpotifyTokenUrl;

                // Initialize request body
                var RequestBody = new Dictionary<string, string>
                {
                  { "grant_type", GrantType },
                  { "refresh_token", OldRefreshToken },
                  { "client_id", ClientID },
                  { "client_secret", ClientSecret }
                };

                // Set Token Info
                var TokenInfo = await _httpService.PostFormUrlEncodedContentAsync(TokenURL, RequestBody);

                // Check existence of Token Assets and return thier values
                var Assets = _sessionService.CheckAssets(TokenInfo);

                // Calculate refresh token expiration date
                string ExpiresIn = _sessionService.CalculateTokenExpirationDate(Assets.ExpiresIn);

                // Store token assets
                _sessionService.StoreAssetes(Assets.AccessToken, Assets.RefreshToken, ExpiresIn);

                // Redirect back to playlists route
                Redirect("/playlists");

                // Successfully redirect back to playlists route
                return Ok("Redirect to playlists route");
            }

            // Redirect back to playlists route
            Redirect("/playlists");

            // Succssful redirection to playlists route
            return Ok("Redirect to playlists route");
        }
    }
}