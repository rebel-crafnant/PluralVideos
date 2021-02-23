using PluralVideos.Download.Resources;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Download.Clients
{
    public class CourseClient : BaseClient
    {
        public CourseClient(Func<bool, Task<string>> getAccessToken, HttpClient httpClientFactory)
            : base(getAccessToken, httpClientFactory) { }

        public async Task<ApiResponse<CourseResource>> GetCourseAsync(string courseName)
            => await GetHttp<CourseResource>($"library/courses/{courseName}");

        public async Task<bool?> HasCourseAccessAsync(string courseName)
        {
            var response = await GetHttp<CourseAccessResource>($"user/courses/{courseName}/access", requiresAuthentication: true);
            return response.Success ? response.Data?.MayDownload : new bool?();
        }
    }
}
