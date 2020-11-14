using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Download.Services.Video
{
    public class VideoClient : BaseClient
    {
        public VideoClient(Func<bool, Task<string>> getAccessToken, HttpClient httpClientFactory)
            : base(getAccessToken, httpClientFactory) { }

        public async Task<ApiResponse<Course>> GetCourse(string courseName) =>
           await GetHttp<Course>($"library/courses/{courseName}");

        public async Task<bool?> HasCourseAccess(string courseName)
        {
            var response = await GetHttp<CourseAccess>($"user/courses/{courseName}/access");
            return response.Success ? response.Data?.MayDownload : new bool?();
        }

        public async Task<ApiResponse<ClipUrls>> GetClipUrls(string courseId, string clipId, bool supportsWideScreen) =>
            await PostHttp<ClipUrls>($"library/videos/offline", new ClipUrlRequest(supportsWideScreen) { ClipId = clipId, CourseId = courseId });
    }
}
