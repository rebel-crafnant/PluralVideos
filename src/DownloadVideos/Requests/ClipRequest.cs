namespace DownloadVideos.Requests
{
    public class ClipRequest
    {
        public string CourseId { get; set; }

        public string ClipId { get; set; }

        public string AspectRatio { get; set; } = "widescreen";
    }
}
