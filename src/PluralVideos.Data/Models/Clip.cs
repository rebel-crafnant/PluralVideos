using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PluralVideos.Data.Models
{
    public class Clip
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Title { get; init; }

        public int ClipIndex { get; init; }

        public int DurationInMilliseconds { get; init; }

        public int SupportsStandard { get; init; }

        public int SupportsWidescreen { get; init; }

        public int ModuleId { get; init; }

        public virtual Module Module { get; init; }

        public virtual ICollection<Transcript> ClipTranscript { get; init; }

        public Clip()
        {
            ClipTranscript = new Collection<Transcript>();
        }
    }
}
