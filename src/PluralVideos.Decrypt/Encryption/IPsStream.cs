using System.IO;

namespace PluralVideos.Decrypt.Encryption
{
    public interface IPsStream
    {
        void Seek(int offset, SeekOrigin begin);

        int Read(byte[] pv, int i, int count);

        void Dispose();

        long Length { get; }

        int BlockSize { get; }
    }
}
