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
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace MusicVault.Services.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IOptions<JwtOptions> options;
        private readonly IConfiguration Configuration;
        public TokenGenerator(IOptions<JwtOptions> opt, IConfiguration config)
        {
            options = opt;
            Configuration = config;
        }
        //старый вариант
        public async Task<string> GenerateAccesseTokenAsync(User user)
        {

            var claims = new[]
            {
                // подправить клаймы
                new Claim(JwtRegisteredClaimNames.Sid,"554546456"),
                new Claim("Id","this default string for test"),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.Login),
                new Claim(JwtRegisteredClaimNames.Email,user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(

                issuer: Configuration["Jwt:Issuer"],
                audience: Configuration["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credential
                );

            return await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(jwtToken));
        }

        // public async Task<string> GenerateAccesseTokenAsync(User user)
        // {
        //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Secret));
        //     var algoritm = SecurityAlgorithms.HmacSha256;
        //     var credentials = new SigningCredentials(key, algoritm);
        //
        //     var claims = new[]
        //     {
        //       //Хранит айди пользователя,можно заменить на Sub константу но поиск геморойный
        //       new Claim("ID",user.Id.ToString()),
        //       new Claim(JwtRegisteredClaimNames.Email,user.Email),
        //       new Claim(JwtRegisteredClaimNames.UniqueName,user.Login),
        //       new Claim(JwtRegisteredClaimNames.Nbf,DateTime.Now.ToString()),
        //       new Claim(JwtRegisteredClaimNames.Exp,DateTime.Now.AddHours(3).ToString())
        //       //пересмотреть инфу, убрать лишнюю
        //   };
        //
        //     var accessToken = new JwtSecurityToken(
        //         issuer: options.Value.Issuer,
        //         audience: options.Value.Audience,
        //         claims: claims,
        //         notBefore: DateTime.Now,
        //         expires: DateTime.Now.AddHours(options.Value.ExpireTime),
        //         signingCredentials: credentials
        //         );
        //
        //     return await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(accessToken));
        //     
        // }

        public async Task<string> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return await Task.Run(() => Convert.ToBase64String(randomNumber));
            }
        }
    }
}
