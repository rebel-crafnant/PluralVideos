using System;
using System.Text.Json.Serialization;

namespace PluralVideos.Download.Resources
{
    public class Register
    {
        public string DeviceModel => "Windows Desktop";

        public string DeviceName { get; init; }
    }

    public class RegisterResource
    {
        [JsonPropertyName("deviceId")]
        public string DeviceId { get; init; }

        [JsonPropertyName("pin")]
        public string Pin { get; init; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; init; }

        [JsonPropertyName("validUntil")]
        public DateTimeOffset ValidUntil { get; init; }

        [JsonPropertyName("serverTime")]
        public DateTimeOffset ServerTime { get; init; }

        [JsonPropertyName("authDeviceUrl")]
        public string AuthDeviceUrl { get; init; }
    }
}
