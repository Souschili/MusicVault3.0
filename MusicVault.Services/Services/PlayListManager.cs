using Microsoft.EntityFrameworkCore;
using MusicVault.Data.Entity;
using MusicVault.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MusicVault.Services.Services
{
    public class PlayListManager : IPlayListManager
    {
        private readonly DbContext context;
        public PlayListManager(DbContext db)
        {
            context = db;
        }

        public async Task CreatePlayListAsynс(string name,string userId)
        {
            var playList = new PlayList { Name = name, OwnerID = userId };
            await context.Set<PlayList>().AddAsync(playList);
            await context.SaveChangesAsync();
        }

        public Task DeletePlayListAsync(string name,string ownerId)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<PlayList>> GetAllPlayListAsync(string ownerId)
        {
            var plList= await context.Set<PlayList>().Where(x=> x.OwnerID==ownerId).ToListAsync();
            return plList;

        }
    }
}
