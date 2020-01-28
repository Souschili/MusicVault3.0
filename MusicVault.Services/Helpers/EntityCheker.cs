using Microsoft.EntityFrameworkCore;
using MusicVault.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MusicVault.Services.Helpers
{
    public class EntityCheker
    {
        private readonly DbContext context;


        public EntityCheker(DbContext db)
        {
            context = db;
        }

        public async Task<bool> CheckLoginAsync(string login)
        {
            var rezult = await context.Set<User>().FirstOrDefaultAsync(x => x.Login == login);
            return rezult != null ? false : true;
        }

        public async Task<bool> ChekEmailAsync(string email)
        {
            var rezult = await context.Set<User>().FirstOrDefaultAsync(x => x.Email == email);
            return rezult != null ? false : true;
        }

        public async Task<bool> CheckPlayListAsync(string plName,List<PlayList> playLists )
        {
            //либо синхронно , либо через юзер айди лезем в бд и чекаем, цена в производительности
            var rezult = await Task.Run(() =>
               playLists.FirstOrDefault(x => x.Name == plName)
              );
            return rezult != null ? false : true;
        }
    }
}
