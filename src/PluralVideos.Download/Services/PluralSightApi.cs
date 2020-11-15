using PluralVideos.Download.Helpers;
using PluralVideos.Download.Services.Auth;
using PluralVideos.Download.Services.Video;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DownloadClient = PluralVideos.Download.Services.Video.DownloadClient;

namespace PluralVideos.Download.Services
{
    public class PluralSightApi
    {
        private readonly int timeout;
        private HttpClient httpClient;
        private User user;

        private AuthClient auth;
        private VideoClient video;
        
        public PluralSightApi(int timeout)
        {
            user = FileHelper.ReadUser();
            this.timeout = timeout;
        }

        public AuthClient Auth => auth ??= new AuthClient(GetAccessToken, HttpClientFactoryInstance);
        public VideoClient Video => video ??= new VideoClient(GetAccessToken, HttpClientFactoryInstance);

        protected async Task<string> GetAccessToken(bool renew = false)
        {
            if (user == null)
                return null;

            if (user.Expiration <= DateTimeOffset.UtcNow.AddDays(1.0) || renew)
            {
                var response = await Auth.GetAccessToken();
                if (response.Success)
                    user = response.Data;
                else
                    user = null;
            }
                

            return user?.Jwt;
        }

        public HttpClient HttpClientFactoryInstance
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
