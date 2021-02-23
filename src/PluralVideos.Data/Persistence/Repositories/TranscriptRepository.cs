using PluralVideos.Data.Core.Repositories;
using PluralVideos.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PluralVideos.Data.Persistence.Repositories
{
    public class TranscriptRepository : Repository<Transcript>, ITranscriptRepository
    {
        public TranscriptRepository(BaseContext context) : base(context)
        {
        }

        public Task<IEnumerable<Transcript>> GetTranscriptsByClip(int clipId)
            => FindAsync(t => t.ClipId == clipId);
    }
}
