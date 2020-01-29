using Microsoft.EntityFrameworkCore;
using MusicVault.Data.Entity;
using MusicVault.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MusicVault.Services.Helpers;

namespace MusicVault.Services.Services
{
    public class PlayListManager : IPlayListManager
    {
        private readonly DbContext context;
        private readonly EntityCheker cheker;
        public PlayListManager(DbContext db,EntityCheker entityCheker)
        {
            context = db;
            cheker = entityCheker;
        }

        public async Task CreatePlayListAsynс(string name,string userId)
        {
            //ищем юзера
            var user = await context.Set<User>().Include(p => p.PlayLists)
                .FirstOrDefaultAsync(x=> x.Id.ToString()==userId);
            //чекаем на уникальность, если такое есть кидаем ошибку
            if (!await cheker.CheckPlayListAsync(name, user.PlayLists))
                throw new EntityValidationException($"We have playlist with {name}");
            //добавляем плейлист
            user.PlayLists.Add(new PlayList { Name = name });
            await context.SaveChangesAsync();
           
        }

        public async Task DeletePlayListAsync(string name,string userId)
        {
            var user = await context.Set<User>().Include(p => p.PlayLists)
                .FirstOrDefaultAsync(x => x.Id.ToString() == userId);
            user.PlayLists.RemoveAll(x => x.Name == name);
            await context.SaveChangesAsync();
        }

        public async Task<ICollection<PlayList>> GetAllPlayListAsync(string userId)
        {

            var user = await context.Set<User>().Include(p => p.PlayLists)
                .FirstOrDefaultAsync(x => x.Id.ToString() == userId);
            return user.PlayLists;
        }

        //удалить 
        public async Task<User> Test(string id)
        {
            //получаем юзера и его плэйлисты
            var user = await context.Set<User>().Include(p=>p.PlayLists)
                .FirstOrDefaultAsync(x => x.Id.ToString() == id);
            user.PlayLists.Add(new PlayList { Name = "Demo" });
            await context.SaveChangesAsync();
            return user;
        }
    }
}
