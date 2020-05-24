using DownloadVideos.Entities;
using Refit;
using System.Threading.Tasks;

namespace DownloadVideos.Apis
{
    public interface ICourseApi
    {
        [Get("/library/courses/{courseName}")]
        Task<Course> GetCourse(string courseName);
    }
}
