using PluralVideos.Data.Persistence;
using PluralVideos.Download.Clients;
using PluralVideos.Download.Resources;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Download
{
    public class DownloadManager
    {
        private readonly TaskQueue queue = new();
        private readonly HttpClient httpClient = new();
        private readonly UnitOfWork<PluralVideosContext> uow = new(new PluralVideosContext());
        private readonly PluralsightApi api;
        private readonly string outputPath;

        public event DownloadClient.DownloadEventHandler OnDownloadEvent;

        public DownloadManager(string outputPath, int timeout)
        {
            var user = uow.Users.GetUserAsync().Result;
            this.api = new PluralsightApi(timeout, user);
            this.outputPath = outputPath;
            this.queue.OnDownloadEvent += OnDownload;
        }

        public void QueueForDownload(HeaderResource course, int moduleId, string moduleTitle, ClipResource clip)
        {
            var client = new DownloadClient(outputPath, course, moduleId, moduleTitle, clip, httpClient, api.GetAccessTokenAsync);
            queue.Enqueue(client);
        }

        public async Task DownloadCourseAsync(bool force) => await queue.DownloadCourseAsync(force);

        public async Task<ApiResponse<CourseResource>> GetCourseAsync(string courseName)
        {
            var course = await uow.Courses.GetCourseAsync(courseName);
            if (course != null)
                return new ApiResponse<CourseResource>
                {
                    Data = CourseResource.FromCourse(course)
                };

            var response = await api.Course.GetCourseAsync(courseName);
            if (response.Success)
            {
                var c = response.Data.ToCourse();
                uow.Courses.Add(c);
                await uow.CompleteAsync();
            }

            return response;
        }

        public async Task<bool> HasAccessAsync(string courseId)
        {
            var response = await api.Course.HasCourseAccessAsync(courseId);
            return response.HasValue && response.Value;
        }

        private void OnDownload(object sender, DownloadEventArg e)
        {
            OnDownloadEvent?.Invoke(sender, e);
        }
    }
}
