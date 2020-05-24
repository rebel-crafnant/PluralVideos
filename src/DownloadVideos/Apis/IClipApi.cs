using DownloadVideos.Requests;
using DownloadVideos.Responses;
using Refit;
using System.Threading.Tasks;

namespace DownloadVideos.Apis
{
    public interface IClipApi
    {
        [Post("/library/videos/offline")]
        Task<ClipResponse> GetClip([Body] ClipRequest request);

        [Get("/user/courses/{courseName}/access")]
        Task<CourseAccessResponse> HasCourseAccess(string courseName);
    }
}
