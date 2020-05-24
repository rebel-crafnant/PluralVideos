using System;
using System.Collections.Generic;
using System.Text;

namespace DownloadVideos.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; }

        public string Jwt { get; set; }

        public string UserHandle { get; set; }

        public DateTimeOffset Expiration { get; set; }
    }
}
