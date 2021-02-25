using PluralVideos.Data.Models;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace PluralVideos.Data.Persistence
{
    public static class FileManager
    {
        public static void CreateTranscript(string outputFolder, string courseTitle, int moduleId, string moduleTitle, Clip clip)
        {
            if (clip.ClipTranscript.Count > 0)
            {
                int i = 1;
                var transcriptPath = $"{GetOutputFilePath(outputFolder, courseTitle, moduleId, moduleTitle, clip.ClipIndex, clip.Title)}.srt";
                using var fs = CreateFile(transcriptPath);
                using var sw = new StreamWriter(fs);
                foreach (var clipTranscript in clip.ClipTranscript)
                {
                    var start = TimeSpan.FromMilliseconds(clipTranscript.StartTime).ToString(@"hh\:mm\:ss\,fff");
                    var end = TimeSpan.FromMilliseconds(clipTranscript.EndTime).ToString(@"hh\:mm\:ss\,fff");
                    sw.WriteLine(i++);
                    sw.WriteLine(start + " --> " + end);
                    sw.WriteLine(clipTranscript.Text);
                    sw.WriteLine();
                }
            }
        }

        public static FileStream CreateVideo(string outputFolder, string courseTitle, int moduleId, string moduleTitle, int clipId, string clipTitle)
        {
            var videoPath = $"{GetOutputFilePath(outputFolder, courseTitle, moduleId, moduleTitle, clipId, clipTitle)}.mp4";
            return CreateFile(videoPath);
        }

        private static string GetOutputFilePath(string outputFolder, string courseTitle, int moduleId, string moduleTitle, int clipId, string clipTitle)
        {
            var decryptedFolder = $@"{outputFolder}\{courseTitle.RemoveInvalidCharacter()}\{moduleId}. {moduleTitle.RemoveInvalidCharacter()}";
            return $@"{decryptedFolder}\{clipId}. {clipTitle.RemoveInvalidCharacter()}";
        }

        private static FileStream CreateFile(string filePath)
        {
            var file = new FileInfo(filePath);
            if (!file.Directory.Exists)
                file.Directory.Create();
            return file.Exists ? file.OpenWrite() : file.Create();
        }

        private static string RemoveInvalidCharacter(this string value)
        {
            var illegal = Path.GetInvalidPathChars();
            var join = $"[{string.Join("", illegal)}]";
            var illegalInFileName = new Regex(join);
            return illegalInFileName.Replace(value.Trim(), "");
        }

        internal static void CreatePluralVideosFolder()
        {
            var di = new DirectoryInfo(PluralVideosBasePath);
            if (!di.Exists)
                di.Create();
        }

        internal static string PluralVideosDb
            => $@"{PluralVideosBasePath}\pluralvideos.db";

        internal static string PluralsightDb
            => $@"{PluralsightBasePath}\pluralsight.db";

        private static string PluralVideosBasePath =>
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\PluralVideos";

        public static string PluralsightBasePath =>
            $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Pluralsight";
    }
}
