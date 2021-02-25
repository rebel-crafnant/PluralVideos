using PluralVideos.Data.Models;
using System.Text.Json.Serialization;

namespace PluralVideos.Download.Resources
{
    public class ClipResource
    {
        [JsonPropertyName("id")]
        public string Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("index")]
        public int Index { get; init; }

        [JsonPropertyName("durationInMilliseconds")]
        public int DurationInMilliseconds { get; init; }

        [JsonPropertyName("supportsStandard")]
        public bool SupportsStandard { get; init; }

        [JsonPropertyName("supportsWideScreen")]
        public bool SupportsWidescreen { get; init; }

        public Clip ToClip()
        {
            return new Clip
            {
                Name = Id,
                Title = Title,
                ClipIndex = Index,
                DurationInMilliseconds = DurationInMilliseconds,
                SupportsStandard = SupportsStandard ? 1 : 0,
                SupportsWidescreen = SupportsWidescreen ? 1 : 0
            };
        }

        public static ClipResource FromClip(Clip clip)
        {
            return new ClipResource
            {
                Id = clip.Name,
                Title = clip.Title,
                Index = clip.ClipIndex,
                DurationInMilliseconds = clip.DurationInMilliseconds,
                SupportsStandard = clip.SupportsStandard != 0,
                SupportsWidescreen = clip.SupportsWidescreen != 0
            };
        }
    }
}
