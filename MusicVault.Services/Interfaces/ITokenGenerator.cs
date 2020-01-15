using MusicVault.Data.Entity;
using System.Threading.Tasks;

namespace MusicVault.Services.Interfaces
{
    public interface ITokenGenerator
    {
        Task<string> GenerateAccesseToken(User user);
        Task<string> GenerateRefreshToken();
    }
}
