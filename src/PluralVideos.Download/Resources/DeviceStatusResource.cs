using System.Text.Json.Serialization;

namespace PluralVideos.Download.Resources
{
    public class DeviceStatusResource
    {
        [JsonPropertyName("status")]
        public string Status { get; init; }
    }
}
