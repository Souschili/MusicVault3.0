using Microsoft.EntityFrameworkCore;
using MusicVault.Data.Entity;
using MusicVault.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicVault.Services.Services
{
    public class UserManager : IUserManager
    {
        private readonly DbContext context;
        public UserManager(DbContext db)
        {
            context = db;
        }

        public async Task AddUser(User user)
        {
            // тут пока все )
            await context.Set<User>().AddAsync(user);
            await context.SaveChangesAsync();
        }



        /// <summary>
        /// тестовый метод
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetUserAsync() =>
            await context.Set<User>().FirstOrDefaultAsync();
    }
}
