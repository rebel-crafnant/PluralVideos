using System;
using System.Text.Json.Serialization;

namespace PluralVideos.Download.Resources 
{
    public class HeaderResource
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("id")]
        public string Id { get; init; }

        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("releaseDate")]
        public string ReleaseDate { get; init; }

        [JsonPropertyName("updatedDate")]
        public string UpdatedDate { get; init; }

        [JsonPropertyName("level")]
        public string Level { get; init; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; init; }

        [JsonPropertyName("defaultImageUrl")]
        public string DefaultImageUrl { get; init; }

        [JsonPropertyName("durationInMilliseconds")]
        public int DurationInMilliseconds { get; init; }

        [JsonPropertyName("shortDescription")]
        public string ShortDescription { get; init; }

        [JsonPropertyName("hasTranscript")]
        public bool HasTranscript { get; init; }
    }
}
