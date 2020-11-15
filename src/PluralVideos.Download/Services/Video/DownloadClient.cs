using PluralVideos.Download.Helpers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Download.Services.Video
{
    public class DownloadClient : BaseClient
    {
        private readonly ClipUrls urls;
        private readonly string outputPath;
        private readonly string courseTitle;
        public int ModuleId { get; }
        public string ModuleTitle { get; }
        public Clip Clip { get; }

        public DownloadClient(ClipUrls urls, string outputPath, string courseTitle, int moduleId, string moduleTitle, Clip clip, HttpClient httpClientFactory, Func<bool, Task<string>> getAccessToken = null) 
            : base(getAccessToken, httpClientFactory)
        {
            this.urls = urls ?? throw new ArgumentNullException(nameof(urls));
            this.outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
            this.courseTitle = courseTitle ?? throw new ArgumentNullException(nameof(courseTitle));
            ModuleId = moduleId;
            ModuleTitle = moduleTitle ?? throw new ArgumentNullException(nameof(moduleTitle));
            Clip = clip ?? throw new ArgumentNullException(nameof(clip));
        }

        public async Task<bool> Download()
        {
            var completed = false;
            foreach (var item in urls.RankedOptions)
            {
                var head = await HeadHttp(item.Url);
                if (!head.Success)
                    continue;

                var filePath = FileHelper.GetVideoPath(outputPath, courseTitle, ModuleId, ModuleTitle, Clip);
                using var fs = FileHelper.CreateFile(filePath);

                var response = await GetFile(item.Url);
                if (response.Success)
                {
                    var buffer = new byte[0x2000];
                    int read;
                    while ((read = response.Data.Read(buffer, 0, buffer.Length)) != 0)
                        fs.Write(buffer, 0, read);

                    completed = true;
                    break;
                }
            }
            
            return completed;
        }

    }
}
