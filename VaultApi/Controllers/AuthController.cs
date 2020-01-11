using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicVault.Data.Entity;
using MusicVault.Services.Interfaces;
using System;
using VaultApi.ViewModels;

namespace VaultApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly IMapper mapper;

        public AuthController(IUserManager manager, IMapper map)
        {
            userManager = manager;
            mapper = map;
        }
        /// <summary>
        /// Регистрация пользователей
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async System.Threading.Tasks.Task<IActionResult> RegisterAsync([FromBody]UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                //чтото придумать с ошибками , автоматом возращается ModelState
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
                // пока ничего не будет но блок обязателен в сервис иди
                return BadRequest(new { error = ex.Message });
            }


            return Ok();
        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("LogIn")]
        public async System.Threading.Tasks.Task<IActionResult> LoginAsync(string login,string password )
        {
            var rezult = await userManager.LogIn(login, password);
            if (!rezult)
                return BadRequest($"Password wrong {password}");

            return Ok("You logIn");
        }
    }
}