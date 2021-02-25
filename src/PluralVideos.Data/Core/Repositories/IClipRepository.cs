using PluralVideos.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PluralVideos.Data.Core.Repositories
{
    public interface IClipRepository : IRepository<Clip>
    {
        Task<IEnumerable<Clip>> GetClipsByModule(int moduleId);
    }
}
