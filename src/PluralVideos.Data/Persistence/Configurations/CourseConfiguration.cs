using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PluralVideos.Data.Models;

namespace PluralVideos.Data.Persistence.Configurations
{
    internal class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(e => e.Name);

            builder.Property(e => e.Name)
                .IsRequired();
        }
    }
}
