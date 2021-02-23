using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PluralVideos.Data.Models;

namespace PluralVideos.Data.Persistence.Configurations
{
    internal class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.Property(e => e.Id);

            builder.HasOne(d => d.Course)
                .WithMany(p => p.Module)
                .HasForeignKey(d => d.CourseName)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
