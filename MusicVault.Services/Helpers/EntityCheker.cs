using Microsoft.EntityFrameworkCore;
using MusicVault.Data.Entity;
using System.Threading.Tasks;

namespace MusicVault.Services.Helpers
{
    public class EntityCheker
    {
        private static DbContext context;


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
    }
}
