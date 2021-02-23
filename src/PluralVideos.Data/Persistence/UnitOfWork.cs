using PluralVideos.Data.Core;
using PluralVideos.Data.Core.Repositories;
using PluralVideos.Data.Persistence.Repositories;
using System.Threading.Tasks;

namespace PluralVideos.Data.Persistence
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : BaseContext
    {
        private readonly TContext context;

        public UnitOfWork(TContext context)
        {
            this.context = context;
            Clips = new ClipRepository(this.context);
            Courses = new CourseRepository(this.context);
            Modules = new ModuleRepository(this.context);
            Transcripts = new TranscriptRepository(this.context);
            Users = new UserRepository(this.context);
        }

        public IClipRepository Clips { get; }

        public ICourseRepository Courses { get; }

        public IModuleRepository Modules { get; }

        public ITranscriptRepository Transcripts { get; }

        public IUserRepository Users { get; }

        public Task CompleteAsync()
            => context.SaveChangesAsync();

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
