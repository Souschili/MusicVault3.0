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
        public UserManager(DbContext db, EntityCheker entityCheker)
        {
            context = db;
            cheker = entityCheker;
        }

        public async Task AddUser(User user, string password)
        {
            // тут пока все )
            if (!await this.cheker.CheckLoginAsync(user.Login))
                throw new EntityValidationException($"Login {user.Login} is using by another user");

            if (!await this.cheker.ChekEmailAsync(user.Email))
                throw new EntityValidationException($"Email {user.Email} is using by another user");

            byte[] passHash, passSalt = new byte[] { };
            await Task.Run(() =>
            {
                PassCryptHelper.CreatePassword(password, out passHash, out passSalt);
                user.PasswordHash = passHash;
                user.PasswordSalt = passSalt;
            });

            await context.Set<User>().AddAsync(user);
            await context.SaveChangesAsync();
        }


        public async Task<bool> LogIn(string login, string password)
        {
            //ищем юзера по логину
            var user = await context.Set<User>().FirstOrDefaultAsync(x=> x.Login==login);
            //TODO подумать,вызывать исключения или возрашать булеан 
            if (user == null) return false;
            // await Task.Run дожидаемся выполнения таска в асинхроном стиле
            return await Task.Run(()=>PassCryptHelper.VerifyPassword(password, user.PasswordSalt, user.PasswordHash));
        }


        /// <summary>
        /// тестовый метод
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetUserAsync() =>
            await context.Set<User>().FirstOrDefaultAsync();


    }
}
