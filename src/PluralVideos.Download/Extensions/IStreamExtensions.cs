using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PluralVideos.Download.Extensions
{
    public static class IStreamExtensions
    {
        public static async Task<T> ReadAndDeserializeFromJson<T>(this HttpResponseMessage message)
        {
            var stream = await message.Content.ReadAsStreamAsync();
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new NotSupportedException("Can't read from this stream.");

            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
    }
}
