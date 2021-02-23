using Microsoft.EntityFrameworkCore;
using PluralVideos.Data.Core.Repositories;
using PluralVideos.Data.Models;
using System.Threading.Tasks;

namespace PluralVideos.Data.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BaseContext context) : base(context)
        {
        }

        public async Task<User> GetUserAsync()
            => await Context.User.FirstOrDefaultAsync();

        public async Task DeleteUserAsync()
        {
            var user = await GetUserAsync();
            Remove(user);
        }
    }
}
