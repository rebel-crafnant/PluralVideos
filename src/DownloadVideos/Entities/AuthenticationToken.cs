using System;

namespace DownloadVideos.Entities
{
    public class AuthenticationToken
    {
        public string Jwt { get; set; }

        public DateTimeOffset Expiration { get; set; }
    }
}
