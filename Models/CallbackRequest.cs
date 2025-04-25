using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.Models
{
    public class CallbackRequest
    {
        /// <summary>
        /// The authorization code returned by the OAuth callback.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Any error message returned by the OAuth callback.
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Any authorization "refresh_token" by the OAuth callback.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}