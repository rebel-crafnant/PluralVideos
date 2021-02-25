using Microsoft.EntityFrameworkCore;
using PluralVideos.Data.Persistence;

namespace PluralVideos.Data.Persistence
{
    public class PluralVideosContext : BaseContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={FileManager.PluralVideosDb}");
    }
}
