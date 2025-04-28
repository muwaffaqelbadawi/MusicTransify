using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.Configuration
{
    public class ApplicationOptions
    {
        public string SpotifyClientId { get; set; } = string.Empty;
        public string SpotifyClientSecret { get; set; } = string.Empty;
        public string SpotifyRedirectURI { get; set; } = string.Empty;
        public string SpotifyAuthURL { get; set; } = string.Empty;
        public string SpotifyTokenURL { get; set; } = string.Empty;
        public string SpotifyAPIBaseURL { get; set; } = string.Empty;
    }
}