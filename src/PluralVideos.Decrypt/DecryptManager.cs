using PluralVideos.Data.Models;
using PluralVideos.Data.Persistence;
using PluralVideos.Decrypt.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PluralVideos.Decrypt
{
    public class DecryptManager
    {
        private readonly UnitOfWork<PluralsightContext> uow;
        private readonly string outputPath;
        private readonly string coursePath;

        public DecryptManager(string outputPath, string databasePath, string coursePath)
        {
            uow = new(new PluralsightContext(databasePath));
            this.outputPath = outputPath;
            this.coursePath = coursePath;
        }

        public Task<IEnumerable<Course>> GetCourses()
            => uow.Courses.GetAllAsync();

        public Task<IEnumerable<Module>> GetModules(string courseName)
            => uow.Modules.GetModuleByCourse(courseName);

        public Task<IEnumerable<Clip>> GetClips(int moduleId)
            => uow.Clips.GetClipsByModule(moduleId);

        public bool CourseFolderExists(string courseName)
            => FileHelper.CourseFolderExists(coursePath, courseName);

        public bool ModuleFolderExists(Module module)
            => FileHelper.ModuleFolderExists(coursePath, module);

        public bool CreateVideo(Clip clip, Module module, string courseTitle)
            => FileHelper.CreateVideo(coursePath, outputPath, courseTitle, module, clip);

        public async Task DeleteCourse(Course course)
        {
            uow.Courses.Remove(course);
            await uow.CompleteAsync();
            FileHelper.DeleteCourseFolder(coursePath, course.Name);
        }

        public void WriteTranscript(Clip clip, Module module, string courseTitle)
            => FileManager.CreateTranscript(outputPath, courseTitle, module.ModuleIndex, module.Title, clip);
    }
}
