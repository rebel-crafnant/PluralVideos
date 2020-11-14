using PluralVideos.Download.Helpers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Download.Services.Video
{
    public class DownloadClient : BaseClient
    {
        public DownloadClient(Func<bool, Task<string>> getAccessToken, HttpClient httpClientFactory) 
            : base(getAccessToken, httpClientFactory) { }

        public async Task<ApiFile> Download(string url, string outputPath, string courseTitle, int moduleId, string moduleTitle, Clip clip)
        {
            var response = await GetFile(url);
            if (response.Success)
            {
                var filePath = FileHelper.GetVideoPath(outputPath, courseTitle, moduleId, moduleTitle, clip);
                using var fs = FileHelper.CreateFile(filePath);

                var buffer = new byte[0x2000];
                int read;
                while ((read = response.Data.Read(buffer, 0, buffer.Length)) != 0)
                    fs.Write(buffer, 0, read);
            }
            return response;
        }
    }
}
