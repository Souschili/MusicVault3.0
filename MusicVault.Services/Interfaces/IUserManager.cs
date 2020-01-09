using MusicVault.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicVault.Services.Interfaces
{
    public interface IUserManager
    {
        Task AddUser(User user);
        Task<User> GetUserAsync();
    }
}
