using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PluralVideos.Download.Helpers
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
            var token = await getToken().ConfigureAwait(false);
            if (token != null)
                request.Headers.Add("ps-jwt", token);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
