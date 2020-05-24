using System.Net;

namespace DownloadVideos.Services
{
    public class RestResponse<T>
    {
        public T Data { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string RawContent { get; set; }
    }
}
