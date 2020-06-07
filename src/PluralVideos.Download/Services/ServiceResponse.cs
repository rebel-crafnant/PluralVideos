namespace PluralVideos.Download.Services
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }

        public bool IsSuccess => Error == null;

        public object Error { get; set; }
    }
}
