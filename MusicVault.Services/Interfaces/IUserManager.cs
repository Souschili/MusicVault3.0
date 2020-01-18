using MusicVault.Data.Entity;
using System.Threading.Tasks;

namespace MusicVault.Services.Interfaces
{
    public interface IUserManager
    {
        Task AddToken(string id,string refresh);
        Task AddUser(User user, string password);
        Task<User> LogIn(string login, string password);
        Task<User> FindUserByLoginAsync(string login);
        Task<User> FindUserByIdAsync(string id);
        Task<User> GetUserAsync();
    }
}
