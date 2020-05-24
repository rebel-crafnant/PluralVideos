using CommandLine;
using System;

namespace DownloadVideos.Option
{
    public class DownloaderOptions
    {
        [Option("out", Required = true, HelpText = "Output folder path")]
        public string OutputPath { get; set; }

        [Option("course", Required = true, HelpText = "Course to download")]
        public string CourseId { get; set; }

        [Option("module", HelpText = "Video clip to download")]
        public string ModuleId { get; set; }

        [Option("clip", HelpText = "Video clip to download")]
        public string ClipId { get; set; }

        [Option("list", HelpText = "List course without downloading")]
        public bool ListCourse { get; set; }

        [Option("db", HelpText = "Database file path")]
        public string DatabasePath { get; set; } = DefaultDatabasePath;

        [Option("timeout", Default = 15, HelpText = "Timeout period for video download in seconds")]
        public int Timeout { get; set; } = 15;

        //[Option("trans", HelpText = "Create subtitle file along with the video")]
        //public bool CreateTranscript { get; set; }

        public bool ListClip
            => !string.IsNullOrEmpty(ClipId) && ListCourse;

        public bool ListModule
            => !string.IsNullOrEmpty(ModuleId) && ListCourse;

        public bool DownloadClip
            => !string.IsNullOrEmpty(ClipId) && !ListCourse;

        public bool DownloadModule
            => !string.IsNullOrEmpty(ModuleId) && !ListCourse;

        public static string BaseUrl => "https://app.pluralsight.com/mobile-api/v2";

        public static string RefreshTokenPath => $"{BaseUrl}/user/authorization/";

        public static string Agent => FileLocator.GetAssembly(BasePath);

        private static string DefaultDatabasePath => $@"{BasePath}\pluralsight.db";

        private static string BasePath =>
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Pluralsight";
    }
}
