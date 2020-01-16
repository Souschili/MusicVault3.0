using MusicVault.Data.Entity;
using MusicVault.Services.DTO;
using System.Threading.Tasks;

namespace MusicVault.Services.Interfaces
{
    public interface ITokenGenerator
    {
        Task<string> GenerateAccesseTokenAsync(User user);
        Task<string> GenerateRefreshTokenAsync();
        Task<TokenDTO> GenerateJwtTokenAsync(User user);
    }
}
