using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PluralVideos.Data.Models
{
    public class Module
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Title { get; init; }

        public string AuthorHandle { get; init; }

        public string Description { get; init; }

        public int DurationInMilliseconds { get; init; }

        public int ModuleIndex { get; init; }

        public string CourseName { get; init; }

        public virtual Course Course { get; init; }

        public virtual ICollection<Clip> Clip { get; init; }

        public Module()
        {
            Clip = new Collection<Clip>();
        }
    }
}
