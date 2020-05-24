using DownloadVideos.Option;
using Microsoft.EntityFrameworkCore;

namespace DownloadVideos.Entities
{
    public partial class PluralSightContext : DbContext
    {
        private readonly DownloaderOptions options;

        public PluralSightContext(DownloaderOptions options)
        {
            this.options = options;
        }

        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={options.DatabasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserHandle);

                entity.OwnsOne(
                    o => o.DeviceInfo,
                    sa =>
                    {
                        sa.Property(p => p.DeviceId).HasColumnName("DeviceId");
                        sa.Property(p => p.RefreshToken).HasColumnName("RefreshToken");
                    });
                
                entity.OwnsOne(
                    o => o.AuthToken,
                    sa =>
                    {
                        sa.Property(p => p.Expiration).HasColumnName("JwtExpiration");
                        sa.Property(p => p.Jwt).HasColumnName("Jwt");
                    });
            });
        }
    }
}
