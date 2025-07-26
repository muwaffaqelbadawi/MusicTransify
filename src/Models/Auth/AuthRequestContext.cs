using System;

namespace MusicTransify.src.Models.Auth
{
    public class AuthRequestContext
    {
        public string State { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string RedirectUri { get; set; } = string.Empty;
    }
}