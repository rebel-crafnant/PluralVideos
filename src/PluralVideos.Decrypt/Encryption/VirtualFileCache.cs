using System;
using System.IO;

namespace PluralVideos.Decrypt.Encryption
{
    class VirtualFileCache : IDisposable
    {
        private readonly IPsStream encryptedVideoFile;

        public long Length => encryptedVideoFile.Length;

        public VirtualFileCache(string encryptedVideoFilePath)
        {
            encryptedVideoFile = new PsStream(encryptedVideoFilePath);
        }

        public VirtualFileCache(IPsStream stream)
        {
            encryptedVideoFile = stream;
        }

        public int Read(byte[] pv, int offset, int count)
        {
            if (Length == 0L)
                return 0;
            encryptedVideoFile.Seek(offset, SeekOrigin.Begin);
            int length = encryptedVideoFile.Read(pv, 0, count);
            VideoEncryption.DecryptBuffer(pv, length, offset);
            return length;
        }

        public void CopyTo(Stream stream)
        {
            var buffer = new byte[0x2000];
            var offset = 0;
            int read;
            while ((read = Read(buffer, offset, buffer.Length)) != 0)
            {
                offset += read;
                stream.Write(buffer, 0, read);
            }
        }

        public void Dispose()
        {
            encryptedVideoFile.Dispose();
        }
    }
}
