using DownloadVideos.Option;
using DownloadVideos.Responses;
using Refit;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DownloadVideos.Services
{
    public class BaseService
    {
        private readonly Repository repository;

        public BaseService(Repository repository)
        {
            this.repository = repository;
        }

        protected async Task<RestResponse<T>> Process<T>(Func<Task<T>> fun) where T : class, new()
        {
            var response = new RestResponse<T>();
            try
            {
                response.Data = await fun();
            }
            catch (ValidationApiException ex)
            {
                response.StatusCode = ex.StatusCode;
            }
            catch (ApiException ex)
            {
                response.StatusCode = ex.StatusCode;
            }

            return response;
        }

        protected async Task<string> GetToken()
        {
            var user = await repository.GetUserAsync() ?? throw new Exception("You have not logged in to pluralsight app");
            if (user.AuthToken.Expiration <= DateTimeOffset.UtcNow.AddDays(1.0))
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(DownloaderOptions.RefreshTokenPath)
                };
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.AddDefaultHeaders();
                var response = await client.PostAsync(user.DeviceInfo.DeviceId,
                    new StringContent(JsonSerializer.Serialize(user.DeviceInfo))).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("There was authentication error.");

                var auth = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var decoded = JsonSerializer.Deserialize<AuthResponse>(auth);
                user.AuthToken.Expiration = decoded.Expiration;
                user.AuthToken.Jwt = decoded.Jwt;
                user.DeviceInfo.RefreshToken = decoded.Token;

                await repository.Context.SaveChangesAsync();
            }

            return user.AuthToken.Jwt;
        }
    }
}
