using DownloadVideos.Entities;
using DownloadVideos.Extensions;
using DownloadVideos.Requests;
using DownloadVideos.Responses;
using DownloadVideos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DownloadVideos.Option
{
    public class Downloader
    {
        private readonly DownloaderOptions options;
        private readonly Repository repository;
        private readonly ClipService clipService;
        private readonly CourseService courseService;
        private readonly HttpClient client;
        private List<Score> scores = new List<Score>
        {
            new Score { Id = 0 },
            new Score { Id = 1 },
            new Score { Id = 2 },
            new Score { Id = 3 }
        };

        public Downloader(DownloaderOptions options)
        {
            this.options = options;
            repository = new Repository(options);
            clipService = new ClipService(repository);
            courseService = new CourseService(repository);
            client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.AddDefaultHeaders();
            client.Timeout = TimeSpan.FromSeconds(options.Timeout);
        }

        public async Task Download()
        {
            if (options.ListClip || options.ListModule)
                Utils.WriteRedText("--list cannot be used with --clip or --module");
            else if (options.DownloadClip)
                await DownloadSingleClipAsync();
            else if (options.DownloadModule)
                await DownloadSingleModuleAsync();
            else
                await DownloadCompleteCourseAsync();
        }

        private async Task DownloadCompleteCourseAsync()
        {
            var course = await GetCourseAsync(options.CourseId, options.ListCourse);

            Utils.WriteYellowText($"Downloading '{course.Header.Title}' started ...");
            Utils.WriteCyanText($"Course Name {course.Header.Name}");

            foreach (var (module, index) in course.Modules.WithIndex())
                await GetModuleAsync(course.Header, module, index, options.ListCourse);

            Utils.WriteYellowText($"Downloading '{course.Header.Title}' completed.");
        }

        private async Task DownloadSingleModuleAsync()
        {
            var course = await GetCourseAsync(options.CourseId, list: false);
            var (module, index) = course.Modules.WithIndex()
                .FirstOrDefault(i => i.item.Id == options.ModuleId);

            Utils.WriteYellowText($"Downloading from course'{course.Header.Title}' started ...");

            await GetModuleAsync(course.Header, module, index, list: false);

            Utils.WriteYellowText($"Download complete");
        }

        private async Task DownloadSingleClipAsync()
        {
            var course = await GetCourseAsync(options.CourseId, list: false);
            var (clip, index, title) = course.GetClip(options.ClipId);

            Utils.WriteYellowText($"Downloading from course'{course.Header.Title}' started ...");
            Utils.WriteGreenText($"\tDownloading from module {index}. {title}");

            await GetClipAsync(clip, course.Header, index, title, list: false);

            Utils.WriteYellowText($"Download complete");
        }

        private async Task<Course> GetCourseAsync(string courseName, bool list)
        {
            var response = await courseService.GetCourse(courseName);
            if (response.Data == null)
                throw new Exception("The course was not found.");
            var hasAccess = await clipService.HasCourseAccess(response.Data.Header.Id);
            var ha = hasAccess.Data?.MayDownload;
            if ((!ha.HasValue || !ha.Value) && !list)
                throw new Exception("You do not have permission to download this course");
            else if ((!ha.HasValue || !ha.Value) && list)
                Utils.WriteRedText("Warning: You do not have permission to download this course");

            return response.Data;
        }

        private async Task GetModuleAsync(Header course, Module module, int index, bool list)
        {
            if (module == null)
                throw new Exception("The module was not found. Check the module and Try again.");

            Utils.WriteGreenText($"\t{index}. {module.Title}", newLine: !list);
            if (list) Utils.WriteCyanText($"  --  {module.Id}");

            foreach (var clip in module.Clips)
                await GetClipAsync(clip, course, index, module.Title, list);
        }

        private async Task GetClipAsync(Clip clip, Header courseHeader, int moduleId, string moduleTitle, bool list)
        {
            if (clip == null)
                throw new Exception("The clip was not found. Check the clip and Try again.");

            Utils.WriteText($"\t\t{clip.Index}. {clip.Title}", newLine: false);
            if (list)
            {
                Utils.WriteCyanText($"  --  {clip.Id}");
                return;
            }

            var response = await clipService.GetClipAsync(new ClipRequest
            {
                CourseId = courseHeader.Id,
                ClipId = clip.Id
            }).ConfigureAwait(false);

            if (response == null)
            {
                Utils.WriteRedText($"\t\t---Error retrieving clip '{clip.Title}'");
                return;
            }

            await DownloadClipAsync(response.Data, courseHeader, moduleId, moduleTitle, clip);
        }

        private async Task DownloadClipAsync(ClipResponse response, Header course, int moduleId, string moduleTitle, Clip clip)
        {
            var scoredOptions = scores.Select(s => response.RankedOptions[s.Id]).ToList(); ;
            foreach (var (item, index) in scoredOptions.WithIndex())
            {
                try
                {
                    if (!(await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, item.Url)).ConfigureAwait(false)).IsSuccessStatusCode)
                    {
                        Utils.WriteRedText($"\t\t---Invalid link: Cdn: {item.Cdn} with Url: {item.Url}");
                        continue;
                    }

                    var downloadResponse = await client.GetAsync(item.Url).ConfigureAwait(false);
                    using var stream = await downloadResponse.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    using var fs = FileLocator.CreateVideo(options.OutputPath, course.Title, moduleId, moduleTitle, clip);

                    var buffer = new byte[0x2000];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) != 0)
                        fs.Write(buffer, 0, read);
                    if (read == 0)
                    {
                        var padding = index != 0 ? "\t\t  " : "  ";
                        Utils.WriteBlueText($"{padding}--  completed");
                        scores.Succeeded(index);
                        break;
                    }
                }
                catch (Exception)
                {
                    var newLine = index == 0 ? "\n" : "";
                    scores.Failed(index);

                    if (index == scoredOptions.Count - 1)
                        Utils.WriteRedText($"\t\t  --  Download failed. To download this video run \n\t\t  '--out <Output Path> --course {course.Name} --clip {clip.Id}'");
                    else
                        Utils.WriteRedText($"{newLine}\t\t  --  Failed: Retry #{index + 1}");
                    continue;
                }
            }
        }
    }
}
