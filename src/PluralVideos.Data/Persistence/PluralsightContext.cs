using Microsoft.EntityFrameworkCore;
using PluralVideos.Data.Persistence;

namespace PluralVideos.Data.Persistence
{
    public class PluralsightContext : BaseContext
    {
        private readonly string databasePath;

        public PluralsightContext(string databasePath = null)
        {
            this.databasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={databasePath ?? FileManager.PluralsightDb}");
    }
}
