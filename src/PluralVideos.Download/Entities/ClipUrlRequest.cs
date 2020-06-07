namespace PluralVideos.Download.Entities
{
    public class ClipUrlRequest
    {
        public string CourseId { get; set; }

        public string ClipId { get; set; }

        public string AspectRatio { get; set; } = "widescreen";
    }
}
