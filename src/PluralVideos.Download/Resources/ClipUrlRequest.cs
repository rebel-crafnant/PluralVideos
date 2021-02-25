namespace PluralVideos.Download.Resources
{
    public class ClipUrlRequest
    {
        public ClipUrlRequest(bool supportsWidescreen)
        {
            AspectRatio = supportsWidescreen ? "widescreen" : "standard";
        }

        public string CourseId { get; init; }

        public string ClipId { get; init; }

        public string AspectRatio { get; }
    }
}
