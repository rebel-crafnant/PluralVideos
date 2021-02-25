using PluralVideos.Data.Models;
using System.Threading.Tasks;

namespace PluralVideos.Data.Core.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<Course> GetCourseAsync(string courseName);
    }
}
