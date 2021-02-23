using PluralVideos.Data.Models;
using PluralVideos.Decrypt;
using PluralVideos.Helpers;
using PluralVideos.Options;
using System.Threading.Tasks;

namespace PluralVideos
{
    public class Decryptor
    {
        private readonly DecryptManager decryptor;
        private readonly DecryptorOptions options;

        public Decryptor(DecryptorOptions options)
        {
            this.options = options;
            decryptor = new DecryptManager(options.OutputPath, options.DatabasePath, options.CoursesPath);
        }

        public async Task RunAsync()
        {
            var courses = await decryptor.GetCourses();
            foreach (var course in courses)
            {
                Utils.WriteYellowText($"Decrypting '{course.Title} started ...");
                if (!decryptor.CourseFolderExists(course.Name))
                {
                    await decryptor.DeleteCourse(course);
                    Utils.WriteRedText($"\tCourse Folder does not exist");
                }
                else
                {
                    var modules = await decryptor.GetModules(course.Name);
                    foreach (var module in modules)
                        await CreateModuleAsync(module, course);

                    Utils.WriteYellowText($"Decrypting '{course.Title}' complete");

                    if (options.RemoveFolderAfterDecryption)
                    {
                        Utils.WriteCyanText($"Removing course '{course.Title}' from database.");
                        await decryptor.DeleteCourse(course);
                        Utils.WriteCyanText($"Removing course '{course.Title}' complete");
                    }
                }
            }
        }

        private async Task CreateModuleAsync(Module module, Course course)
        {
            Utils.WriteGreenText($"\t{module.ModuleIndex}. {module.Title}");
            if (!decryptor.ModuleFolderExists(module))
                Utils.WriteRedText($"\tModule Folder does not exist");
            else
            {
                var clips = await decryptor.GetClips(module.Id);
                foreach (var clip in clips)
                {
                    Utils.WriteText($"\t\t{clip.ClipIndex}. {clip.Title}");
                    if (!decryptor.CreateVideo(clip, module, course.Title))
                    {
                        Utils.WriteRedText($"\t\t{clip.Title} does not exist");
                        return;
                    }

                    if (options.CreateTranscript && course.HasTranscript == 1)
                    {
                        Utils.WriteBlueText($"\t\t----Writing '{clip.Title}' transcript.");
                        decryptor.WriteTranscript(clip, module, course.Title);
                    }
                }
            }
        }
    }
}