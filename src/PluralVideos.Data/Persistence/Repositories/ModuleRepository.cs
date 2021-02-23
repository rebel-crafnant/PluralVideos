using PluralVideos.Data.Core.Repositories;
using PluralVideos.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PluralVideos.Data.Persistence.Repositories
{
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        public ModuleRepository(BaseContext context) : base(context)
        {
        }

        public Task<IEnumerable<Module>> GetModuleByCourse(string courseName)
            => FindAsync(m => m.CourseName == courseName);
    }
}
