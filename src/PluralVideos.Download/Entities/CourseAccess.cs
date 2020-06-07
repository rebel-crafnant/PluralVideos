using Newtonsoft.Json;

namespace PluralVideos.Download.Entities
{
    public class CourseAccess
    {
        [JsonProperty(PropertyName = "mayDownload")]
        public bool MayDownload { get; set; }
    }
}
