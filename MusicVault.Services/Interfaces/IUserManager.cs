using MusicVault.Data.Entity;
using System.Threading.Tasks;

namespace MusicVault.Services.Interfaces
{
    public interface IUserManager
    {
        Task AddUser(User user, string password);
        Task<bool> LogIn(string login, string password);
        Task<User> GetUserAsync();
    }
}
