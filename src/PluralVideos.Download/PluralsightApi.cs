using PluralVideos.Data.Models;
using PluralVideos.Download.Clients;
using PluralVideos.Download.Resources;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PluralVideos.Download
{
    public class PluralsightApi
    {
        private readonly int timeout;
        private HttpClient httpClient;
        private User user;

        private AuthClient auth;
        private CourseClient course;

        public PluralsightApi(int timeout, User user)
        {
            this.timeout = timeout;
            this.user = user;
        }

        public AuthClient Auth => auth ??= new AuthClient(GetAccessTokenAsync, HttpClientFactoryInstance);
        public CourseClient Course => course ??= new CourseClient(GetAccessTokenAsync, HttpClientFactoryInstance);

        public async Task<string> GetAccessTokenAsync(bool renew = false)
        {
            if (user?.JwtExpiration <= DateTimeOffset.UtcNow.AddDays(1.0) || renew)
            {
                var response = await Auth.AuthorizeAsync(new DeviceInfoResource { DeviceId = user.DeviceId, RefreshToken = user.RefreshToken });
                user = response.Success ? response.Data.ToUser() : null;
            }

            return user?.Jwt;
        }

        private HttpClient HttpClientFactoryInstance
        {
            get
            {
                if (httpClient == null)
                {
                    httpClient = new HttpClient
                    {
                        Timeout = TimeSpan.FromSeconds(timeout)
                    };
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "WPF/1.0.282");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    ServicePointManager.DefaultConnectionLimit = 10;
                }

                return httpClient;
            }
        }
    }
}
