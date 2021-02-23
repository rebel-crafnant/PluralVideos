namespace PluralVideos.Data.Models
{
    public class Transcript
    {
        public int Id { get; init; }

        public int StartTime { get; init; }

        public int EndTime { get; init; }

        public string Text { get; init; }

        public int ClipId { get; init; }

        public virtual Clip Clip { get; init; }
    }
}
