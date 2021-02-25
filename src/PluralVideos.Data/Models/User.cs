using System;

namespace PluralVideos.Data.Models
{
    public class User
    {
        public string DeviceId { get; set; }

        public string RefreshToken { get; set; }

        public string Jwt { get; set; }

        public DateTimeOffset JwtExpiration { get; set; }

        public string UserHandle { get; set; }

        public bool IsLocal { get; set; }
    }
}
