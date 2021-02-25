namespace PluralVideos.Download.Clients
{
    public record DownloadEventArg(string ModuleTitle, int ModuleIndex, string ClipTitle, int ClipIndex, long Size, long Progess);
}
