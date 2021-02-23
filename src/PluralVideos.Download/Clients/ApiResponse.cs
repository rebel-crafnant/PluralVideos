using PluralVideos.Download.Extensions;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PluralVideos.Download.Clients
{
    public class ApiResponse
    {
        public bool Success => Error == null;

        public HttpResponseMessage Message { get; set; }

        public ApiError Error { get; set; }


        public static async Task<ApiResponse> FromMessage(HttpResponseMessage message)
        {
            var response = new ApiResponse { Message = message };
            if (!message.IsSuccessStatusCode)
                await response.HandleFailedCall();

            return response;
        }

        protected async Task HandleFailedCall()
        {
            try
            {
                Error = await Message.ReadAndDeserializeFromJson<ApiError>() ?? new ApiError();
                if (Error.Message == null)
                    Error = new () { Message = Message.StatusCode.ToString() };
            }
            catch
            {
                Error = new() { Message = Message.StatusCode.ToString() };
            }
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }

        public new static async Task<ApiResponse<T>> FromMessage(HttpResponseMessage message)
        {
            var response = new ApiResponse<T> { Message = message };
            if (message.IsSuccessStatusCode)
                response.Data = await message.ReadAndDeserializeFromJson<T>();
            else
                await response.HandleFailedCall();

            return response;
        }
    }

    public class ApiFile : ApiResponse<Stream>
    {
        public new static async Task<ApiFile> FromMessage(HttpResponseMessage message)
        {
            var response = new ApiFile { Message = message };
            if (message.IsSuccessStatusCode)
                response.Data = await message.Content.ReadAsStreamAsync();
            else
                await response.HandleFailedCall();

            return response;
        }
    }

    public class ApiError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string PropertyName { get; set; }
    }
}
