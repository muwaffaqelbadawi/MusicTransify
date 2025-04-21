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
        public string? Code { get; init; }

        /// <summary>
        /// Any error message returned by the OAuth callback.
        /// </summary>
        public string? Error { get; init; }
    }
}