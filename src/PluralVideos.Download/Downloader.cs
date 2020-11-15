using PluralVideos.Download.Extensions;
using PluralVideos.Download.Helpers;
using PluralVideos.Download.Options;
using PluralVideos.Download.Services;
using PluralVideos.Download.Services.Video;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PluralVideos.Download
{
    public class Downloader
    {
        private readonly DownloaderOptions options;

        private readonly TaskQueue queue;

        private readonly PluralSightApi api;

        public Downloader(DownloaderOptions options)
        {
            if (options.Threads > 10) throw new InvalidOperationException();

            this.options = options;
            api = new PluralSightApi(options.Timeout);
            queue = new TaskQueue(options.Threads);
            queue.ProcessCompleteEvent += ClipDownloaded;
        }

        public async Task Download()
        {
            if (options.ListClip || options.ListModule)
                Utils.WriteRedText("--list cannot be used with --clip or --module");

            var course = await GetCourseAsync(options.CourseId, list: false);
            Utils.WriteYellowText($"Downloading from course'{course.Header.Title}' started ...");
            Utils.WriteGreenText($"\tPreparing files for download ...");

            if (options.DownloadClip)
            {
                var (clip, index, title) = course.GetClip(options.ClipId);
                await GetClipAsync(clip, course.Header, index, title, list: false);
            }
            else if (options.DownloadModule)
            {
                var (module, index) = course.Modules.WithIndex()
                .FirstOrDefault(i => i.item.Id == options.ModuleId);

                await GetModuleAsync(course.Header, module, index, list: false);
            }
            else
            {
                foreach (var (module, index) in course.Modules.WithIndex())
                    await GetModuleAsync(course.Header, module, index, options.ListCourse);
            }

            Utils.WriteGreenText($"\tDownloading has started ...");

            await queue.Execute();

            Utils.WriteYellowText($"Download complete");
        }

        private async Task<Course> GetCourseAsync(string courseName, bool list)
        {
            var courseResponse = await api.Video.GetCourse(courseName);
            if (!courseResponse.Success)
                throw new Exception($"Course was not found. Error: {courseResponse.ResponseBody}");

            var hasAccess = await api.Video.HasCourseAccess(courseResponse.Data.Header.Id);
            var noAccess = (!hasAccess.HasValue || !hasAccess.Value);
            if (noAccess  && !list)
                throw new Exception("You do not have permission to download this course");
            else if (!noAccess&& list)
                Utils.WriteRedText("Warning: You do not have permission to download this course");

            return courseResponse.Data;
        }

        private async Task GetModuleAsync(Header course, Module module, int index, bool list)
        {
            if (module == null)
                throw new Exception("The module was not found. Check the module and Try again.");

            if (list)
            {
                Utils.WriteGreenText($"\t{index}. {module.Title}", newLine: false);
                Utils.WriteBlueText($"  --  {module.Id}");
            }

            foreach (var clip in module.Clips)
                await GetClipAsync(clip, course, index, module.Title, list);
        }

        private async Task GetClipAsync(Clip clip, Header course, int moduleId, string moduleTitle, bool list)
        {
            if (clip == null)
                throw new Exception("The clip was not found. Check the clip and Try again.");

            if (list)
            {
                Utils.WriteText($"\t\t{clip.Index}. {clip.Title}", newLine: false);
                Utils.WriteCyanText($"  --  {clip.Id}");
                return;
            }

            var response = await api.Video.GetClipUrls(course.Id, clip.Id, clip.SupportsWidescreen);
            if (!response.Success)
            {
                Utils.WriteRedText($"\n\t\t---Error retrieving clip '{clip.Title}'. Message: {response.Error.Message}");
                return;
            }

            var client = new DownloadClient(response.Data, options.OutputPath, course.Title, moduleId, moduleTitle, clip, api.HttpClientFactoryInstance);
            queue.Enqueue(client);
        }

        private void ClipDownloaded(object sender, DownloadEventArgs e)
        {
            Utils.WriteGreenText($"\n\t{e.ModuleId}. {e.ModuleTitle}");
            if (e.Succeeded)
                Utils.WriteText($"\t\t{e.ClipId}. {e.ClipTitle}  --  downloaded");
            else
                Utils.WriteRedText($"\t\t{e.ClipId}. {e.ClipTitle} --  Download failed. will retry again.");
        }
    }
}
