using CommandLine;
using PluralVideos.Decrypt.Helpers;
using PluralVideos.Decrypt.Options;
using System;

namespace PluralVideos.Decrypt
{
    static class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<DecryptorOptions>(args)
                .WithParsed(RunOptions);
        }

        private static void RunOptions(DecryptorOptions options)
        {
            try
            {
                var decryptor = new Decryptor(options);
                decryptor.DecryptCourse();
            }
            catch (Exception exception)
            {
                Utils.WriteRedText($"Error occured: {exception.Message}");
            }
        }
    }
}