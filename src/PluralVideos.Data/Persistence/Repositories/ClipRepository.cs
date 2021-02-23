using Microsoft.EntityFrameworkCore;
using PluralVideos.Data.Core.Repositories;
using PluralVideos.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PluralVideos.Data.Persistence.Repositories
{
    public class ClipRepository : Repository<Clip>, IClipRepository
    {
        public ClipRepository(BaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Clip>> GetClipsByModule(int moduleId)
            => await Context.Clip
                .Include(c => c.ClipTranscript)
                .Where(c => c.ModuleId == moduleId)
                .ToListAsync();
    }
}
