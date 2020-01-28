using MusicVault.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicVault.Services.Interfaces
{
    public interface IPlayListManager
    {
        Task CreatePlayListAsynс(string name,string userId);
        Task DeletePlayListAsync(string name,string ownerId);
        Task<ICollection<PlayList>> GetAllPlayListAsync(string userId); 
        Task<User> Test(string id); //delete
    }
}
