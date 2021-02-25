using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PluralVideos.Data.Models;

namespace PluralVideos.Data.Persistence.Configurations
{
    internal class ClipConfiguration : IEntityTypeConfiguration<Clip>
    {
        public void Configure(EntityTypeBuilder<Clip> builder)
        {
            builder.Property(e => e.Id);

            builder.HasOne(d => d.Module)
                .WithMany(p => p.Clip)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
