using CommandLine;
using PluralVideos.Data;
using PluralVideos.Helpers;
using PluralVideos.Options;
using System;
using System.Threading.Tasks;

namespace PluralVideos
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Database.Setup();

            var result = Parser.Default.ParseArguments<AuthenticatorOptions, DecryptorOptions, DownloaderOptions>(args);
            await result.WithParsedAsync<AuthenticatorOptions>(RunAuthOptions);
            await result.WithParsedAsync<DownloaderOptions>(RunDownloadOptions);
            await result.WithParsedAsync<DecryptorOptions>(RunDecryptorOptions);
        }

        private static async Task RunAuthOptions(AuthenticatorOptions options)
        {
            try
            {
                var authenticator = new Authenticator(options);
                await authenticator.RunAsync();
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
                await downloader.RunAsync();
            }
            catch (Exception exception)
            {
                Utils.WriteRedText($"Error occured: {exception.Message}");
            }
        }

        private static async Task RunDecryptorOptions(DecryptorOptions options)
        {
            try
            {
                var decryptor = new Decryptor(options);
                await decryptor.RunAsync();
            }
            catch (Exception exception)
            {
                Utils.WriteRedText($"Error occured: {exception.Message}");
            }
        }
    }
}
