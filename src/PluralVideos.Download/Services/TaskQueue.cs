using PluralVideos.Download.Services.Video;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PluralVideos.Download.Services
{
    // Thank to https://codereview.stackexchange.com/questions/145938/semaphore-based-concurrent-work-queue
    public class DownloadEventArgs : EventArgs
    {
        public DownloadEventArgs(bool succeeded, int moduleId, string moduleTitle, int clipId, string clipTitle)
        {
            Succeeded = succeeded;
            ModuleId = moduleId;
            ModuleTitle = moduleTitle;
            ClipId = clipId;
            ClipTitle = clipTitle;
        }

        public bool Succeeded { get; set; }

        public int ModuleId { get; }

        public string ModuleTitle { get; set; }

        public int ClipId { get; set; }

        public string ClipTitle { get; set; }
    }

    public sealed class TaskQueue : IDisposable
    {
        private readonly SemaphoreSlim semaphore;

        private readonly ConcurrentQueue<DownloadClient> clients = new ConcurrentQueue<DownloadClient>();

        public event EventHandler<DownloadEventArgs> ProcessCompleteEvent;

        public TaskQueue() : this(1)
        { }

        public TaskQueue(int degreeOfParallelism)
        {
            semaphore = new SemaphoreSlim(degreeOfParallelism);
        }

        public void Enqueue(DownloadClient client)
        {
            clients.Enqueue(client);
        }

        public async Task Execute()
        {
            var tasks = new List<Task>();
            while (clients.TryDequeue(out var client))
            {
                await semaphore.WaitAsync();
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var th = this;
                        var completed = await client.Download();
                        if (!completed) clients.Enqueue(client);
                        OnRaiseDownloadEvent(new DownloadEventArgs(completed, client.ModuleId, client.ModuleTitle, client.Clip.Index, client.Clip.Title));
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }

        public void Dispose()
        {
            semaphore.Dispose();
        }

        private void OnRaiseDownloadEvent(DownloadEventArgs e)
        {
            ProcessCompleteEvent?.Invoke(this, e);
        }
    }
}
