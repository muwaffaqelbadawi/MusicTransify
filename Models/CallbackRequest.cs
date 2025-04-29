using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.Models
{
    public class CallbackRequest
    {
        public string? Code { get; set; }
        public string? Error { get; set; }
    }
}