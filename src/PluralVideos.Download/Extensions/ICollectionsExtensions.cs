using PluralVideos.Download.Resources;
using System.Collections.Generic;
using System.Linq;

namespace PluralVideos.Download.Extensions
{
    public static class ICollectionsExtensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }

        public static (ClipResource, int, string) GetClip(this CourseResource course, string clipId)
        {
            var items = course.Modules.Select((m, index) => (m.Clips.FirstOrDefault(c => c.Id == clipId), index, m.Title));
            return items.FirstOrDefault(i => i.Item1 != null);
        }
    }
}
