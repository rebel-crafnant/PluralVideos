using System;
using System.Linq;
using System.Threading.Tasks;
using PluralVideos.Download.Clients;
using PluralVideos.Download.Extensions;
using PluralVideos.Download;
using PluralVideos.Options;
using PluralVideos.Helpers;
using Konsole;
using System.Collections.Generic;
using PluralVideos.Download.Resources;

namespace PluralVideos
{
    public class Downloader
    {
        private readonly Dictionary<int, ProgressBar> bars = new();
        private readonly DownloaderOptions options;
        private readonly DownloadManager manager;

        public Downloader(DownloaderOptions options)
        {
            this.options = options;
            this.manager = new DownloadManager(options.OutputPath, options.Timeout);
            this.manager.OnDownloadEvent += OnDownload;
        }

        public async Task RunAsync()
        {
            if (options.ListClip || options.ListModule)
                Utils.WriteRedText("--list cannot be used with --clip or --module");

            var course = await GetCourseAsync(options.CourseId, list: false);
            Utils.WriteYellowText($"Downloading from course'{course.Header.Title}' started ...");

            if (options.DownloadClip)
            {
                var (clip, index, title) = course.GetClip(options.ClipId);
                GetClipAsync(clip, course.Header, index, title, list: false);
            }
            else if (options.DownloadModule)
            {
                var (module, index) = course.Modules.WithIndex()
                    .FirstOrDefault(i => i.item.Id == options.ModuleId);

                GetModuleAsync(course.Header, module, index, list: false);
            }
            else
            {
                foreach (var (module, index) in course.Modules.WithIndex())
                    GetModuleAsync(course.Header, module, index, options.ListCourse);
            }

            Utils.WriteGreenText($"Downloading has started ...");

            await manager.DownloadCourseAsync(options.Force);

            Utils.WriteYellowText($"Download complete");
        }

        private void OnDownload(object sender, DownloadEventArg e)
        {
            var key = e.ModuleIndex * 100 + e.ClipIndex;
            if (!bars.TryGetValue(key, out var bar))
            {
                bar = new ProgressBar((int)e.Size);
                bars.Add(key, bar);
            }

            bar.Refresh((int)e.Progess, $"{key:D3}. {e.ClipTitle}");
        }

        private async Task<CourseResource> GetCourseAsync(string courseName, bool list)
        {
            var response = await manager.GetCourseAsync(courseName);
            if (!response.Success)
                throw new Exception($"Course was not found.");

            var hasAccess = await manager.HasAccessAsync(response.Data.Header.Id);
            if (!hasAccess  && !list)
                throw new Exception("You do not have permission to download this course");
            else if (hasAccess && list)
                Utils.WriteRedText("Warning: You do not have permission to download this course");

            return response.Data;
        }

        private void GetModuleAsync(HeaderResource course, ModuleResource module, int index, bool list)
        {
            if (module == null)
                throw new Exception("The module was not found. Check the module and Try again.");

            if (list)
            {
                Utils.WriteGreenText($"\t{index}. {module.Title}", newLine: false);
                Utils.WriteBlueText($"  --  {module.Id}");
            }

            foreach (var clip in module.Clips)
                GetClipAsync(clip, course, index, module.Title, list);
        }

        private void GetClipAsync(ClipResource clip, HeaderResource course, int moduleId, string moduleTitle, bool list)
        {
            if (clip == null)
                throw new Exception("The clip was not found. Check the clip and Try again.");

            if (list)
            {
                Utils.WriteText($"\t\t{clip.Index}. {clip.Title}", newLine: false);
                Utils.WriteCyanText($"  --  {clip.Id}");
                return;
            }

            manager.QueueForDownload(course, moduleId, moduleTitle, clip);
        }
    }
}
