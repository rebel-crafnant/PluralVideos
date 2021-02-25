using System.Text.Json.Serialization;

namespace PluralVideos.Download.Resources
{
    public class DeviceInfoResource
    {
        [JsonPropertyName("deviceId")]
        public string DeviceId { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
