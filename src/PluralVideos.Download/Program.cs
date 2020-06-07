using CommandLine;
using PluralVideos.Download.Helpers;
using PluralVideos.Download.Options;
using System;
using System.Threading.Tasks;

namespace PluralVideos.Download
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<AuthenticatorOptions, DownloaderOptions>(args);
            await result.WithParsedAsync<AuthenticatorOptions>(RunAuthOptions);
            await result.WithParsedAsync<DownloaderOptions>(RunDownloadOptions);
        }

        private static async Task RunAuthOptions(AuthenticatorOptions options)
        {
            try
            {
                var authenticator = new Authenticator(options);
                await authenticator.Run();
            }
            catch (Exception exception)
            {
                Utils.WriteRedText($"Error occured: {exception.Message}");
            }
        }

        private static async Task RunDownloadOptions(DownloaderOptions options)
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
