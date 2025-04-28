using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SpotifyWebAPI_Intro.Configuration;

namespace SpotifyWebAPI_Intro.Services
{
    public class OptionsService
    {
        // Primary constructor
        private readonly ApplicationOptions _options;

        public OptionsService(IOptions<ApplicationOptions> options)
        {
            _options = options.Value;
        }

        // Options properties
        public string SpotifyClientId => _options.SpotifyClientId;
        public string SpotifyClientSecret => _options.SpotifyClientSecret;
        public string SpotifyRedirectUri => _options.SpotifyRedirectURI;
        public string SpotifyAuthUrl => _options.SpotifyAuthURL;
        public string SpotifyTokenUrl => _options.SpotifyTokenURL;
        public string SpotifyApiBaseUrl => _options.SpotifyAPIBaseURL;
    }
}