using Microsoft.EntityFrameworkCore;
using MusicVault.Data.Entity;
using MusicVault.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicVault.Services.Services
{
    public class PlayListManager : IPlayListManager
    {
        private readonly DbContext context;
        public PlayListManager(DbContext db)
        {
            context = db;
        }

        public async Task CreatePlayListAsync(string name, string userId)
        {
            var playList = new PlayList { Name = name, OwnerID = userId };
            await context.Set<PlayList>().AddAsync(playList);
            await context.SaveChangesAsync();
        }

        public Task DeletePlayList(string name)
        {
            throw new NotImplementedException();
        }
    }
}
