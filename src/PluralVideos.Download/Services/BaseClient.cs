using PluralVideos.Download.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PluralVideos.Download.Services
{
    public class BaseClient
    {
        private const string BaseUri = "https://app.pluralsight.com/mobile-api/v2";
        private readonly Func<bool, Task<string>> getAccessToken;
        private readonly HttpClient httpClient;

        public BaseClient(Func<bool, Task<string>> getAccessToken, HttpClient httpClientFactory)
        {
            this.getAccessToken = getAccessToken;
            this.httpClient = httpClientFactory;
        }

        protected async Task<ApiResponse<T>> GetHttp<T>(string url)
        {
            return await SendHttp<T>(() => new HttpRequestMessage(HttpMethod.Get, $"{BaseUri}/{url}"));
        }

        protected async Task<ApiResponse<T>> PostHttp<T>(string url, object data)
        {
            return await SendHttp<T>(() => new HttpRequestMessage(HttpMethod.Post, $"{BaseUri}/{url}")
            {
                Content = HttpClientHelpers.GetJsonBody(data)
            });
        }

        protected async Task<ApiResponse> DeleteHttp(string url)
        {
            return await SendHttp(() => new HttpRequestMessage(HttpMethod.Delete, $"{BaseUri}/{url}"));
        }

        protected async Task<ApiResponse> HeadHttp(string url)
        {
            return await SendHttp(() => new HttpRequestMessage(HttpMethod.Head, url));
        }

        protected async Task<ApiFile> GetFile(string url)
        {
            return await SendHttpFile(() => new HttpRequestMessage(HttpMethod.Get, url));
        }

        private async Task<ApiResponse> SendHttp(Func<HttpRequestMessage> requestFunc)
        {
            try
            {
                var request = requestFunc();
                await SetAuthHeader(request, false);

                var response = await httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    request = requestFunc();
                    await SetAuthHeader(request, true);
                    response = await httpClient.SendAsync(request);
                }

                return await ApiResponse.FromMessage(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Error = new ApiError { Message = GetRecursiveErrorMessage(ex) }
                };
            }
        }

        private async Task<ApiResponse<T>> SendHttp<T>(Func<HttpRequestMessage> requestFunc)
        {
            try
            {
                var request = requestFunc();
                await SetAuthHeader(request, false);

                var response = await httpClient.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    request = requestFunc();
                    await SetAuthHeader(request, true);
                    response = await httpClient.SendAsync(request);
                }

                return await ApiResponse<T>.FromMessage(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>()
                {
                    Error = new ApiError { Message = GetRecursiveErrorMessage(ex) }
                };
            }
        }

        private async Task<ApiFile> SendHttpFile(Func<HttpRequestMessage> requestFunc)
        {
            try
            {
                var request = requestFunc();
                await SetAuthHeader(request, false);

                var response = await httpClient.SendAsync(request);

                return await ApiFile.FromMessage(response);
            }
            catch (Exception ex)
            {
                return new ApiFile()
                {
                    Error = new ApiError { Message = GetRecursiveErrorMessage(ex) }
                };
            }
        }

        private async Task SetAuthHeader(HttpRequestMessage message, bool renew)
        {
            if (getAccessToken != null)
            {
                var token = await getAccessToken(renew);
                if (!string.IsNullOrEmpty(token))
                    message.Headers.Add("ps-jwt", token);
            }
        }

        private string GetRecursiveErrorMessage(Exception ex, string delimeter = " --- ")
        {
            var sb = new StringBuilder();
            var currentException = ex;
            while (currentException != null)
            {
                if (!string.IsNullOrEmpty(sb.ToString()))
                    sb.Append(delimeter);
                sb.Append(currentException.Message);

                currentException = currentException.InnerException;
            }

            return sb.ToString();
        }
    }
}
