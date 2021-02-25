using PluralVideos.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PluralVideos.Data.Core.Repositories
{
    public interface ITranscriptRepository : IRepository<Transcript>
    {
        Task<IEnumerable<Transcript>> GetTranscriptsByClip(int clipId);
    }
}
