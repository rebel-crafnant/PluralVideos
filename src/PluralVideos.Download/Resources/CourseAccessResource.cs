using System.Text.Json.Serialization;

namespace PluralVideos.Download.Resources
{
    public class CourseAccessResource
    {
        [JsonPropertyName("mayDownload")]
        public bool? MayDownload { get; init; }
    }
}
