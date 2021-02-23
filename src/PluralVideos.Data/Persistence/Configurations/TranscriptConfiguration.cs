using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PluralVideos.Data.Models;

namespace PluralVideos.Data.Persistence.Configurations
{
    internal class TranscriptConfiguration : IEntityTypeConfiguration<Transcript>
    {
        public void Configure(EntityTypeBuilder<Transcript> builder)
        {
            builder.ToTable("ClipTranscript");

            builder.HasIndex(e => e.StartTime)
                    .HasDatabaseName("index_ClipTranscriptStart");

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.EndTime).HasColumnType("integer");

            builder.HasOne(d => d.Clip)
                .WithMany(p => p.ClipTranscript)
                .HasForeignKey(d => d.ClipId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
