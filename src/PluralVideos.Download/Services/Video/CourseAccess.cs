using Newtonsoft.Json;

namespace PluralVideos.Download.Services.Video
{
    public class CourseAccess
    {
        [JsonProperty(PropertyName = "mayDownload")]
        public bool? MayDownload { get; set; }
    }
}
