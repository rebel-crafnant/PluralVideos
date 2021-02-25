using CommandLine;

namespace PluralVideos.Options
{
    [Verb("decrypt", HelpText = "Decrypts courses downloaded by pluralsight app")]
    public class DecryptorOptions
    {
        [Option("out", Required = true, HelpText = "Output folder path")]
        public string OutputPath { get; set; }

        [Option("db", HelpText = "Database file path")]
        public string DatabasePath { get; set; }

        [Option("course", HelpText = "Course folder path")]
        public string CoursesPath { get; set; }

        [Option("trans", HelpText = "Create subtitle file along with the video")]
        public bool CreateTranscript { get; set; }

        [Option("rm", HelpText = "Remove encrypted folder after decryption")]
        public bool RemoveFolderAfterDecryption { get; set; }
    }
}
