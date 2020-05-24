using DownloadVideos.Apis;
using DownloadVideos.Entities;
using DownloadVideos.Option;
using Refit;
using System.Threading.Tasks;

namespace DownloadVideos.Services
{
    public class CourseService : BaseService
    {
        protected readonly ICourseApi api;

        public CourseService(Repository repository) : base(repository)
        {
            api = RestService.For<ICourseApi>(DownloaderOptions.BaseUrl);
        }

        public async Task<RestResponse<Course>> GetCourse(string courseName)
        {
            return await Process(async () =>
            {
                return await api.GetCourse(courseName);
            });
        }
    }
}
