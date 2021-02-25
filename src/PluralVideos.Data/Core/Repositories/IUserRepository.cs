using PluralVideos.Data.Models;
using System.Threading.Tasks;

namespace PluralVideos.Data.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserAsync();
        Task DeleteUserAsync();
    }
}
