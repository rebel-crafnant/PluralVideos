using DownloadVideos.Option;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadVideos
{
    public class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        private readonly Func<Task<string>> getToken;

        public AuthenticatedHttpClientHandler(Func<Task<string>> getToken)
        {
            this.getToken = getToken ?? throw new ArgumentNullException(nameof(getToken));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.AddDefaultHeaders();
            request.Headers.Add("ps-jwt", await getToken().ConfigureAwait(false));

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }

    public static class HttpRewuestExtensions 
    {
        public static void AddDefaultHeaders(this HttpRequestHeaders headers)
        {
            headers.Accept.ParseAdd("application/json");
            headers.UserAgent.ParseAdd(DownloaderOptions.Agent);
        }
    }
}
