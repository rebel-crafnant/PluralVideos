using PluralVideos.Download.Apis;
using PluralVideos.Download.Entities;
using PluralVideos.Download.Helpers;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Download.Services
{
    public class PluralsightService : BaseService
    {
        protected readonly IPluralsightApi api;

        private bool RequiresAuthentication { get; set; }

        public PluralsightService()
        {
            api = RestService.For<IPluralsightApi>(new HttpClient(new AuthenticatedHttpClientHandler(GetToken))
            {
                BaseAddress = new Uri("https://app.pluralsight.com/mobile-api/v2")
            });
        }

        public async Task<Course> GetCourseAsync(string courseName)
        {
            var course = FileHelper.ReadCourse(courseName);
            if (course != null)
                return course;

            RequiresAuthentication = false;
            var response = await Process(async () => await api.GetCourse(courseName));
            if (!response.IsSuccess)
                return null;

            course = response.Data;
            FileHelper.WriteCourse(course);

            return course;
        }

        public async Task<RegisterResponse> AuthenticateAsync()
        {
            RequiresAuthentication = false;
            var response = await Process(async () => await api.Authenticate(new Register()));
            if (!response.IsSuccess)
                return null;
            return response.Data;
        }

        public async Task<ServiceResponse<ClipUrls>> GetClipUrlsAsync(string courseId, string clipId)
        {
            RequiresAuthentication = true;
            return await Process(async () =>
                await api.GetClipUrls(new ClipUrlRequest { CourseId = courseId, ClipId = clipId }));
        }

        public async Task<bool> HasCourseAccess(string courseId)
        {
            RequiresAuthentication = true;
            var response = await Process(async () => await api.HasCourseAccess(courseId));
            if (!response.IsSuccess)
                throw new Exception("There was an error retrieving access.");
            return response.Data.MayDownload;
        }

        public async Task<bool> DeviceStatus(string deviceId)
        {
            RequiresAuthentication = false;
            var response = await Process(async () => await api.DeviceStatus(deviceId));
            if (!response.IsSuccess)
                throw new Exception("There was an error retrieving access.");
            return response.Data.Status == "Valid";
        }

        public async Task<bool> Logout()
        {
            var user = FileHelper.ReadUser();
            if (user == null)
                return false;

            RequiresAuthentication = true;
            var response = await Process(async () => await api.Logout(user.DeviceInfo.DeviceId));
            if (!response.IsSuccess)
                throw new Exception("There was an error logging out.");
            FileHelper.DeleteUser();
            return true;
        }

        public async Task<User> AuthorizeAsync(User user)
        {
            RequiresAuthentication = false;
            var deviceInfo = user.DeviceInfo;
            var response = await Process(async () => await api.Authorize(deviceInfo.DeviceId, deviceInfo));
            if (!response.IsSuccess)
                throw new Exception("There was authorization error.");

            user.Expiration = response.Data.Expiration;
            user.Jwt = response.Data.Jwt;
            user.UserHandle = response.Data.UserHandle;

            FileHelper.WriteUser(user);

            return user;
        }

        private async Task<string> GetToken()
        {
            if (!RequiresAuthentication)
                return null;

            var user = FileHelper.ReadUser() ?? throw new Exception("You have not logged in to pluralsight app");
            if (user.Expiration <= DateTimeOffset.UtcNow.AddDays(1.0))
                user = await AuthorizeAsync(user);

            return user.Jwt;
        }
    }
}
