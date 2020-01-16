using MusicVault.Data.Entity;
using System.Threading.Tasks;

namespace MusicVault.Services.Interfaces
{
    public interface ITokenGenerator
    {
        Task<string> GenerateAccesseTokenAsync(User user);
        Task<string> GenerateRefreshTokenAsync();
    }
}
