using CommandLine;
using DecryptVideos.Option;
using System;

namespace DecryptVideos
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