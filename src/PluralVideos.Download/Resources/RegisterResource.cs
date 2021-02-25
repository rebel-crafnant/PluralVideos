using System;
using System.Text.Json.Serialization;

namespace PluralVideos.Download.Resources
{
    public class UnauthenticatedRegister
    {
        public string DeviceModel => "Windows Desktop";

        public string DeviceName { get; init; }
    }

    public class AuthenticatedRegister
    {
        public string DeviceModel => "Windows Desktop";

        public string DeviceName { get; init; }

        public string Username { get; init; }

        public string Password { get; init; }
    }

    public class RegisterResource : DeviceInfoResource
    {
        [JsonPropertyName("pin")]
        public string Pin { get; init; }

        [JsonPropertyName("validUntil")]
        public DateTimeOffset ValidUntil { get; init; }

        [JsonPropertyName("serverTime")]
        public DateTimeOffset ServerTime { get; init; }

        [JsonPropertyName("authDeviceUrl")]
        public string AuthDeviceUrl { get; init; }
    }
}
