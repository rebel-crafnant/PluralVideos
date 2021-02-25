using PluralVideos.Download.Resources;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Download.Clients
{
    public class AuthClient : BaseClient
    {
        public AuthClient(Func<bool, Task<string>> getAccessToken, HttpClient httpClientFactory)
            : base(getAccessToken, httpClientFactory)
        {
        }

        public async Task<ApiResponse<RegisterResource>> AutheticateAsync()
            => await PostHttp<RegisterResource>("user/device/unauthenticated", new UnauthenticatedRegister());

        public async Task<ApiResponse<DeviceInfoResource>> AutheticateAsync(string username, string password)
            => await PostHttp<DeviceInfoResource>("user/device/authenticated", new AuthenticatedRegister
            {
                Username = username,
                Password = password
            });

        public async Task<ApiResponse<UserResource>> AuthorizeAsync(DeviceInfoResource deviceInfo)
            => await PostHttp<UserResource>($"user/authorization/{deviceInfo.DeviceId}", deviceInfo);

        public async Task<ApiResponse<DeviceStatusResource>> GetStatusAsync(string deviceId)
            => await GetHttp<DeviceStatusResource>($"user/device/{deviceId}/status");

        public async Task<ApiResponse> LogoutAsync(string deviceId)
            => await DeleteHttp($"user/device/{deviceId}");
    }
}
