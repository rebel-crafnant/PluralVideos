using PluralVideos.Download.Entities;
using Refit;
using System.Threading.Tasks;

namespace PluralVideos.Download.Apis
{
    [Headers("Accept: application/json", "User-Agent: WPF/1.0.247.0")]
    public interface IPluralsightApi
    {
        [Post("/library/videos/offline")]
        Task<ClipUrls> GetClipUrls([Body] ClipUrlRequest request);

        [Get("/user/courses/{courseName}/access")]
        Task<CourseAccess> HasCourseAccess(string courseName);

        [Get("/library/courses/{courseName}")]
        Task<Course> GetCourse(string courseName);

        [Post("/user/device/unauthenticated")]
        Task<RegisterResponse> Authenticate([Body] Register register);

        [Get("/user/device/{deviceId}/status")]
        Task<DeviceStatus> DeviceStatus(string deviceId);

        [Post("/user/authorization/{deviceId}")]
        Task<User> Authorize(string deviceId, [Body] DeviceInfo deviceInfo);
    }
}
