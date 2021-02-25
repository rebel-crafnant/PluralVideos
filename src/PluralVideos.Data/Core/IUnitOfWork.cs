using PluralVideos.Data.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace PluralVideos.Data.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IClipRepository Clips { get; }

        ICourseRepository Courses { get; }

        IModuleRepository Modules { get; }

        ITranscriptRepository Transcripts { get; }

        IUserRepository Users { get; }

    Task CompleteAsync();
    }
}
