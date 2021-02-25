using PluralVideos.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PluralVideos.Data.Core.Repositories
{
    public interface IModuleRepository : IRepository<Module>
    {
        Task<IEnumerable<Module>> GetModuleByCourse(string courseName);
    }
}
