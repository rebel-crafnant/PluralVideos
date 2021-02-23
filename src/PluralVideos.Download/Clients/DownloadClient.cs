using PluralVideos.Data.Persistence;
using PluralVideos.Download.Resources;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Download.Clients
{
    public class DownloadClient : BaseClient
    {
        private readonly string outputPath;
        private readonly HeaderResource course;
        private readonly int moduleId;
        private readonly string moduleTitle;
        private readonly ClipResource clip;

        public delegate void DownloadEventHandler(object sender, DownloadEventArg e);

        public event DownloadEventHandler OnDownloadEvent;

        public DownloadClient(string outputPath, HeaderResource course, int moduleId, string moduleTitle, ClipResource clip, HttpClient httpClientFactory, Func<bool, Task<string>> getAccessToken = null)
            : base(getAccessToken, httpClientFactory)
        {
            this.outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
            this.course = course ?? throw new ArgumentNullException(nameof(course));
            this.moduleId = moduleId;
            this.moduleTitle = moduleTitle ?? throw new ArgumentNullException(nameof(moduleTitle));
            this.clip = clip ?? throw new ArgumentNullException(nameof(clip));
        }

        public async Task<bool> DownloadVideoAsync(bool force = false)
        {
            var clipsResponse = await GetClipUrlsAsync();
            if (!clipsResponse.Success)
                return false;

            var completed = false;
            foreach (var item in clipsResponse.Data.RankedOptions)
            {
                var head = await HeadHttp(item.Url);
                if (!head.Success)
                    continue;

                using var fs = FileManager.CreateVideo(outputPath, course.Title, moduleId, moduleTitle, clip.Index, clip.Title);
                if (fs.Length > 0 && fs.Length == head.Data && !force)
                {
                    OnDownloadEvent?.Invoke(this, new(moduleTitle, moduleId, clip.Title, clip.Index, head.Data, head.Data));
                    return true;
                }

                var response = await GetFile(item.Url);
                if (response.Success)
                {
                    var buffer = new byte[0x2000];
                    int read;
                    int progress = 0;
                    while ((read = response.Data.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        fs.Write(buffer, 0, read);
                        progress += read;
                        OnDownloadEvent?.Invoke(this, new(moduleTitle, moduleId, clip.Title, clip.Index, head.Data, progress));
                    }
  
                    completed = true;
                    break;
                }
            }
            return completed;
        }

        private async Task<ApiResponse<ClipUrlsResource>> GetClipUrlsAsync() =>
            await PostHttp<ClipUrlsResource>($"library/videos/offline", new ClipUrlRequest(clip.SupportsWidescreen) { ClipId = clip.Id, CourseId = course.Id }, requiresAuthentication: true);

    }
}
