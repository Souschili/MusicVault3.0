using Microsoft.Extensions.Options;
using MusicVault.Data.Entity;
using MusicVault.Services.Helpers;
using MusicVault.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace MusicVault.Services.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IOptions<JwtOptions> options;
        public TokenGenerator(IOptions<JwtOptions> opt)
        {
            options = opt;
        }

        public async Task<string> GenerateAccesseToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Secret));
            var algoritm = SecurityAlgorithms.HmacSha256;
            var credentials = new SigningCredentials(key, algoritm);

            var claims = new[]
            {
              //Хранит айди пользователя,можно заменить на Sub константу но поиск геморойный
              new Claim("ID",user.Id.ToString()),
              new Claim(JwtRegisteredClaimNames.Email,user.Email),
              new Claim(JwtRegisteredClaimNames.UniqueName,user.Login),
              new Claim(JwtRegisteredClaimNames.Nbf,DateTime.Now.ToString()),
              new Claim(JwtRegisteredClaimNames.Exp,DateTime.Now.AddHours(3).ToString())
              //пересмотреть инфу, убрать лишнюю
          };

            var accessToken = new JwtSecurityToken(
                issuer: options.Value.Issuer,
                audience: options.Value.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(options.Value.ExpireTime),
                signingCredentials: credentials
                );
            var token= await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(accessToken));
            return token;
        }

        public async Task<string> GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}
