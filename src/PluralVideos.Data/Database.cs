using Microsoft.EntityFrameworkCore;
using PluralVideos.Data.Persistence;

namespace PluralVideos.Data
{
    public static class Database
    {
        public static void Setup()
        {
            FileManager.CreatePluralVideosFolder();
            using var pluralVideosDb = new PluralVideosContext();
            pluralVideosDb.Database.Migrate();
        }
    }
}
