using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.Models
{
    /// <summary>
    /// Represents a request from Spotify's OAuth callback.
    /// </summary>
    public class CallbackRequest
    {
        /// <summary>
        /// The authorization code returned by Spotify.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Any error message returned by Spotify.
        /// </summary>
        public string? Error { get; set; }
    }
}