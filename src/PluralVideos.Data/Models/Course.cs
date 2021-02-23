using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PluralVideos.Data.Models
{
    public class Course
    {
        public string Name { get; init; }

        public string Title { get; init; }

        public string ReleaseDate { get; init; }

        public string UpdatedDate { get; init; }

        public string Level { get; init; }

        public string ShortDescription { get; init; }

        public string Description { get; init; }

        public int DurationInMilliseconds { get; init; }

        public int HasTranscript { get; init; }

        public string AuthorsFullnames { get; init; }

        public string ImageUrl { get; init; }

        public string DefaultImageUrl { get; init; }

        public int? IsStale { get; init; }

        public string CachedOn { get; init; }

        public string UrlSlug { get; init; }

        public virtual ICollection<Module> Module { get; init; }

        public Course()
        {
            Module = new Collection<Module>();
        }
    }
}
