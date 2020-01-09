using Microsoft.EntityFrameworkCore;
using MusicVault.Data.Entity;
using MusicVault.Services.Helpers;
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
        private readonly EntityCheker cheker;
        public UserManager(DbContext db,EntityCheker entityCheker)
        {
            context = db;
            cheker = entityCheker;
        }

        public async Task AddUser(User user)
        {
            // тут пока все )
            if (!await this.cheker.CheckLoginAsync(user.Login))
                throw new EntityValidationException($"Login {user.Login} is using by another user");

            if (!await this.cheker.ChekEmailAsync(user.Email))
                throw new EntityValidationException($"Email {user.Email} is using by another user");

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
