using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using PluralVideos.Decrypt.Encryption;
using PluralVideos.Data.Persistence;
using PluralVideos.Data.Models;

namespace PluralVideos.Decrypt.Helpers
{
    internal static class FileHelper
    {
        public static bool CourseFolderExists(string coursesFolder, string courseName)
        {
            var di = new DirectoryInfo($@"{CourseFolder(coursesFolder, courseName)}");
            return di.Exists;
        }

        public static void DeleteCourseFolder(string coursesFolder, string courseName)
        {
            var coursePath = CourseFolder(coursesFolder, courseName);
            var di = new DirectoryInfo(coursePath);
            if (!di.Exists)
                return;
            di.Delete(recursive: true);
        }

        public static bool ModuleFolderExists(string coursesFolder, Module module)
        {
            var modulePath = ModuleFolder(coursesFolder, module);

            var di = new DirectoryInfo(modulePath);
            return di.Exists;
        }

        public static bool CreateVideo(string coursesFolder, string outputFolder, string courseTitle, Module module, Clip clip)
        {
            var encryptedVideoPath = $@"{ModuleFolder(coursesFolder, module)}/{clip.Name}.psv";
            var encryptedFileInfo = new FileInfo(encryptedVideoPath);
            if (!encryptedFileInfo.Exists)
                return false;

            using var fs = FileManager.CreateVideo(outputFolder, courseTitle, module.ModuleIndex, module.Title, clip.ClipIndex, clip.Title);
            var stream = new VirtualFileStream(encryptedVideoPath);
            stream.Read(fs);

            return true;
        }

        private static string CourseFolder(string coursesPath, string courseName)
            => $@"{coursesPath ?? DefaultCoursesPath}\{courseName}";

        private static string ModuleFolder(string coursesFolder, Module module)
        {
            string s = $"{module.Name}|{module.AuthorHandle}";
            using var md5 = MD5.Create();
            var moduleHash = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(s))).Replace('/', '_');
            return $@"{CourseFolder(coursesFolder, module.CourseName)}\{moduleHash}";
        }


        private static string DefaultCoursesPath => $@"{FileManager.PluralsightBasePath}\courses";
    }
}
