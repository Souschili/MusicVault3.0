using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MusicVault.Services.Interfaces
{
    public interface IPlayListManager
    {
        Task CreatePlayListAsynс(string name,string userId);
        Task DeletePlayList(string name);
    }
}
