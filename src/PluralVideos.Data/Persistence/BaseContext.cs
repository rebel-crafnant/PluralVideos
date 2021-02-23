using Microsoft.EntityFrameworkCore;
using PluralVideos.Data.Models;
using System.Reflection;

namespace PluralVideos.Data.Persistence
{
    public class BaseContext : DbContext
    {
        public virtual DbSet<Clip> Clip { get; set; }

        public virtual DbSet<Transcript> Transcript { get; set; }

        public virtual DbSet<Course> Course { get; set; }

        public virtual DbSet<Models.Module> Module { get; set; }

        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseContext).Assembly);
        }
    }
}
