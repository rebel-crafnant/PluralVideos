using PluralVideos.Data.Models;
using PluralVideos.Download.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace PluralVideos.Download.Resources
{
    public class CourseResource
    {
        [JsonPropertyName("header")]
        public HeaderResource Header { get; init; }

        [JsonPropertyName("description")]
        public string Description { get; init; }

        [JsonPropertyName("modules")]
        public virtual ICollection<ModuleResource> Modules { get; init; }

        public Course ToCourse()
        {
            return new Course
            {
                Name = Header.Id,
                Title = Header.Title,
                ReleaseDate = Header.ReleaseDate,
                UpdatedDate = Header.UpdatedDate,
                Level = Header.Level,
                ShortDescription = Header.ShortDescription,
                Description = Description,
                DurationInMilliseconds = Header.DurationInMilliseconds,
                HasTranscript = Header.HasTranscript ? 1 : 0,
                AuthorsFullnames = "",
                ImageUrl = Header.ImageUrl,
                DefaultImageUrl = Header.DefaultImageUrl,
                IsStale = null,
                CachedOn = DateTimeOffset.UtcNow.ToString(),
                UrlSlug = Header.Name,
                Module = Modules.WithIndex().Select(m => m.item.ToModule(Header.Id, m.index)).ToList()
            };
        }

        public static CourseResource FromCourse(Course course)
        {
            return new CourseResource
            {
                Header = new HeaderResource
                {
                    Id = course.Name,
                    Name = course.UrlSlug,
                    Title = course.Title,
                    ReleaseDate = course.ReleaseDate,
                    UpdatedDate = course.UpdatedDate,
                    Level = course.Level,
                    ShortDescription = course.ShortDescription,
                    DurationInMilliseconds = course.DurationInMilliseconds,
                    HasTranscript = course.HasTranscript != 0,
                    ImageUrl = course.ImageUrl,
                    DefaultImageUrl = course.DefaultImageUrl,
                },
                Description = course.Description,
                Modules = course.Module.Select(m => ModuleResource.FromModule(m)).ToList()
            };
        }
    }
}
