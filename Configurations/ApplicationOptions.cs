using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.Configuration
{
    public class ApplicationOptions
    {
        public string SpotifyClientId { get; init;} = string.Empty;
        public string SpotifyClientSecret { get; init;} = string.Empty;
        public string SpotifyRedirectURI { get; init;} = string.Empty;
        public string SpotifyAauthURL { get; init;} = string.Empty;
        public string SpotifyTokenURL { get; init;} = string.Empty;
        public string SpotifyAPIBaseURL { get; init;} = string.Empty;
    }
}