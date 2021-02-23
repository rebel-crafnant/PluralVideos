using Microsoft.EntityFrameworkCore;
using PluralVideos.Data.Core.Repositories;
using PluralVideos.Data.Models;
using System.Threading.Tasks;

namespace PluralVideos.Data.Persistence.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(BaseContext context) : base(context)
        {
        }

        public Task<Course> GetCourseAsync(string courseName)
            => Context.Course
                .Include(c => c.Module)
                    .ThenInclude(m => m.Clip)
                .FirstOrDefaultAsync(c => c.UrlSlug == courseName);
    }
}
