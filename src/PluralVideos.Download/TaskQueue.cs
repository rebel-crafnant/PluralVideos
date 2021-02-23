using PluralVideos.Download.Clients;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace PluralVideos.Download
{
    // Thank to https://codereview.stackexchange.com/questions/145938/semaphore-based-concurrent-work-queue
    // https://markheath.net/post/constraining-concurrent-threads-csharp

    public sealed class TaskQueue : IDisposable
    {
        private readonly SemaphoreSlim semaphore;

        private readonly ConcurrentQueue<DownloadClient> clients = new();
        private readonly ConcurrentBag<Task> tasks = new();

        public event DownloadClient.DownloadEventHandler OnDownloadEvent;

        public TaskQueue()
        {
            var concurrentDownloads = Math.Max(5, Environment.ProcessorCount * 2);
            semaphore = new SemaphoreSlim(concurrentDownloads);
        }

        public void Enqueue(DownloadClient client)
        {
            client.OnDownloadEvent += Client_OnDownloadEvent;
            clients.Enqueue(client);
        }

        public async Task DownloadCourseAsync(bool force)
        {
            while (clients.TryDequeue(out var client))
            {
                if (client != null)
                {
                    await semaphore.WaitAsync();
                    tasks.Add(Task.Run(async () => await DownloadAsync(client, force)));
                }
            }

            await Task.WhenAll(tasks);
        }

        public void Dispose()
        {
            semaphore.Dispose();
        }

        private async Task DownloadAsync(DownloadClient client, bool force)
        {
            try
            {
                if (!await client.DownloadVideoAsync(force))
                    clients.Enqueue(client);
            }
            finally
            {
                semaphore.Release();
            }
        }

        private void Client_OnDownloadEvent(object sender, DownloadEventArg e)
        {
            OnDownloadEvent?.Invoke(sender, e);
        }
    }
}
