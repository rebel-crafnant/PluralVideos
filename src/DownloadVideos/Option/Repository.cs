using DownloadVideos.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DownloadVideos.Option
{
    public class Repository : IDisposable
    {
        public PluralSightContext Context { get; }

        public Repository(DownloaderOptions options)
        {
            Context = new PluralSightContext(options);
        }

        public async Task<User> GetUserAsync()
        {
            return await Context.User
                .FirstOrDefaultAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
