using DownloadVideos.Entities;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DownloadVideos.Option
{
    public static class FileLocator
    {
        public static string GetAssembly(string basePath)
        {
            var di = new DirectoryInfo(basePath);
            if (!di.Exists)
                throw new Exception("Folder does not exist");
            var directories = di.GetDirectories();
            var names = directories.Where(n => n.Name.StartsWith("app-"))
                .OrderByDescending(n => n.Name)
                .FirstOrDefault();

            return string.Format("WPF/{0}", (object)System.Reflection.Assembly.LoadFrom($"{names.FullName}\\Pluralsight.exe").GetName().Version);
        }

        public static FileStream CreateVideo(string outputFolder, string courseTitle, int moduleId, string moduleTitle, Clip clip)
        {
            var file = new FileInfo($@"{outputFolder}\{courseTitle.RemoveInvalidCharacter()}\{moduleId}. {moduleTitle.RemoveInvalidCharacter()}\{clip.Index}. {clip.Title.RemoveInvalidCharacter()}.mp4");
            if (!file.Directory.Exists)
                file.Directory.Create();
            return file.Create();
        }

        private static string RemoveInvalidCharacter(this string value)
        {
            var illegalInFileName = new Regex(@"[\\/:*?""<>|]");
            return illegalInFileName.Replace(value.Trim(), "");
        }
    }
}
