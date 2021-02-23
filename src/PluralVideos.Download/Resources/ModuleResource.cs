using PluralVideos.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace PluralVideos.Download.Resources
{
    public partial class ModuleResource
    {
        [JsonPropertyName("id")]
        public string Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("authorHandle")]
        public string AuthorHandle { get; init; }

        [JsonPropertyName("description")]
        public string Description { get; init; }

        [JsonPropertyName("durationInMilliseconds")]
        public int DurationInMilliseconds { get; init; }

        [JsonPropertyName("hasLearningChecks")]
        public bool HasLearningChecks { get; init; }

        [JsonPropertyName("learningChecksCount")]
        public int LearningChecksCount { get; init; }

        [JsonPropertyName("clips")]
        public virtual ICollection<ClipResource> Clips { get; init; }

        public Module ToModule(string courseId, int index)
        {
            return new Module
            {
                Name = Id,
                Title = Title,
                AuthorHandle = AuthorHandle,
                Description = Description,
                DurationInMilliseconds = DurationInMilliseconds,
                ModuleIndex = index,
                CourseName = courseId,
                Clip = Clips.Select(c => c.ToClip()).ToList()
            };
        }

        public static ModuleResource FromModule(Module module)
        {
            return new ModuleResource
            {
                Id = module.Name,
                Title = module.Title,
                AuthorHandle = module.AuthorHandle,
                Description = module.Description,
                DurationInMilliseconds = module.DurationInMilliseconds,
                Clips = module.Clip.Select(c => ClipResource.FromClip(c)).ToList()
            };
        }
    } 
}
