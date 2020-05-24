using DownloadVideos.Apis;
using DownloadVideos.Option;
using DownloadVideos.Requests;
using DownloadVideos.Responses;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DownloadVideos.Services
{
    public class ClipService : BaseService
    {
        protected readonly IClipApi api;

        public ClipService(Repository repository) : base(repository)
        {
            api = RestService.For<IClipApi>(new HttpClient(new AuthenticatedHttpClientHandler(GetToken)) 
            {
                BaseAddress = new Uri(DownloaderOptions.BaseUrl)
            });
        }

        public async Task<RestResponse<ClipResponse>> GetClipAsync(ClipRequest request)
        {
            return await Process(async () => 
            {
                return await api.GetClip(request);
            });
        }

        public async Task<RestResponse<CourseAccessResponse>> HasCourseAccess(string courseName)
        {
            return await Process(async () =>
            {
                return await api.HasCourseAccess(courseName);
            });
        }
    }
}
