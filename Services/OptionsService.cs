using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SpotifyWebAPI_Intro.Configuration;

namespace SpotifyWebAPI_Intro.Services
{
    public class OptionsService(IOptions<ApplicationOptions> options)
    {
        private readonly ApplicationOptions _options = options.Value;

        public string SpotifyClientId => _options.SpotifyClientId;
        public string SpotifyClientSecret => _options.SpotifyClientSecret;
        public string SpotifyRedirectUri => _options.SpotifyRedirectURI;
        public string SpotifyAuthUrl => _options.SpotifyAauthURL;
        public string SpotifyTokenUrl => _options.SpotifyTokenURL;
        public string SpotifyApiBaseUrl => _options.SpotifyAPIBaseURL;
    }
}