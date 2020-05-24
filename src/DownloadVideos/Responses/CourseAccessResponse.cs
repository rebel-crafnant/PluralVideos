using Newtonsoft.Json;

namespace DownloadVideos.Responses
{
    public class CourseAccessResponse
    {
        [JsonProperty(PropertyName = "mayDownload")]
        public bool MayDownload { get; set; }
    }
}
