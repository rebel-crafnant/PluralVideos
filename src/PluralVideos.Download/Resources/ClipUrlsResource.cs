using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PluralVideos.Download.Resources
{
    public class ClipUrlsResource
    {
        [JsonPropertyName("url")]
        public string Url { get; init; }

        [JsonPropertyName("rankedOptions")]
        public List<UrlOption> RankedOptions { get; init; }
    }

    public class UrlOption
    {
        [JsonPropertyName("cdn")]
        public string Cdn { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }
    }
}
