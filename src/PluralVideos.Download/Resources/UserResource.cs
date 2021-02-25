using PluralVideos.Data.Models;
using System;
using System.Text.Json.Serialization;

namespace PluralVideos.Download.Resources
{
    public class UserResource
    {
        public DeviceInfoResource DeviceInfo { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; init; }

        [JsonPropertyName("expiration")]
        public DateTimeOffset Expiration { get; init; }

        [JsonPropertyName("userHandle")]
        public string UserHandle { get; init; }

        public User ToUser()
        {
            return new User
            {
                Jwt = Token,
                JwtExpiration = Expiration,
                UserHandle = UserHandle,
                DeviceId = DeviceInfo.DeviceId,
                RefreshToken = DeviceInfo.RefreshToken
            };
        }
    }
}
