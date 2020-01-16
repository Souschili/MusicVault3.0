using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicVault.Data.Entity;
using MusicVault.Services.Interfaces;
using System;
using System.Threading.Tasks;
using VaultApi.ViewModels;

namespace VaultApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly IMapper mapper;
        private readonly ITokenGenerator tokenGenerator;

        public AuthController(IUserManager manager, IMapper map, ITokenGenerator generator)
        {
            userManager = manager;
            mapper = map;
            tokenGenerator = generator;
        }
        /// <summary>
        /// Регистрация пользователей
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                //чтото придумать с ошибками , автоматом возращается ModelState.errors
                return BadRequest();
            }
            // если все ок то маппим нашу вьюху в юзера 
            var user = mapper.Map<User>(model);

            try
            {
                await userManager.AddUser(user, model.Password);
            }
            catch (Exception ex)
            {
                // TODO общий класс ошибок
                return BadRequest(new { error = ex.Message });
            }
            //возращаем токены(модель не нужна)
            var token = await tokenGenerator.GenerateAccesseTokenAsync(user);
            var refresh = await tokenGenerator.GenerateRefreshTokenAsync();

            return Ok(new
            {
                token,
                refresh
            });

        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("LogIn")]
        public async Task<IActionResult> LoginAsync(string login, string password)
        {
            var rezult = await userManager.LogIn(login, password);
            if (!rezult)
                //для теста,переделать на "логин или пароль неверный" 
                return BadRequest($"Password wrong {password}");

            var user = await userManager.GetUserAsync();

            var token = await tokenGenerator.GenerateAccesseTokenAsync(user);
            var refresh = await tokenGenerator.GenerateRefreshTokenAsync();

            return Ok(new
            {
                token,
                refresh
            });


        }
    }
}