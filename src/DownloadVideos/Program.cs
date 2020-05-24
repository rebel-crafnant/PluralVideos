using CommandLine;
using DownloadVideos.Option;
using System;
using System.Threading.Tasks;

namespace DownloadVideos
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<DownloaderOptions>(args)
                .WithParsedAsync(RunOptions);
        }

        private static async Task RunOptions(DownloaderOptions options)
        {
            try
            {
                var downloader = new Downloader(options);
                await downloader.Download();
            }
            catch (Exception exception)
            {
                Utils.WriteRedText($"Error occured: {exception.Message}");
            }
        }
    }
}
