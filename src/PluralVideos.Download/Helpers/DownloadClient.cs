using PluralVideos.Download.Entities;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Download.Helpers
{
    public class DownloadClient : IDisposable
    {
        private readonly HttpClient client;

        public DownloadClient(int timeout)
        {
            client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(timeout)
            };
            client.DefaultRequestHeaders.Clear();
        }

        public async Task<bool> Download(UrlOption item, string filePath)
        {
            if (!(await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, item.Url)).ConfigureAwait(false)).IsSuccessStatusCode)
                return false;

            var downloadResponse = await client.GetAsync(item.Url).ConfigureAwait(false);
            using var stream = await downloadResponse.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var fs = FileHelper.CreateFile(filePath);

            var buffer = new byte[0x2000];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) != 0)
                fs.Write(buffer, 0, read);

            return true;
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
