using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MusicVault.Data.Entity;
using MusicVault.Services.Helpers;
using MusicVault.Services.Interfaces;
using System;
using System.Threading.Tasks;
using VaultApi.ViewModels;

namespace VaultApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IOptions<JwtOptions> options;
        private readonly ITokenGenerator generator;

        public TestController(IMapper map,IOptions<JwtOptions> opt)
        {
            mapper = map;
            options = opt;
        }
        [HttpGet("GetUser")]
        public IActionResult Get()
        {
            //из этого получим юзера
            var userView = new UserRegistrationModel { Email = "test@basic.er", Login = "Xaos", Password = "17456123" };
            //вот так вспомнил наконецто
            var user=mapper.Map<User>(userView);
            return Ok(user);
        }

        [HttpGet("Config")]
        public IActionResult get()
        {
            return Ok(options.Value);
        }

        [HttpGet]
        public async Task<IActionResult> TokenDTOAsync()
        {
            //левый юзер
            var user = new User
            {
                Create=DateTime.Now,
                Login="Geronimo",
                Email="shubnigurat@dark.org",
                Id=System.Guid.NewGuid(),
                //пароли опускаем
            };

            var token = await generator.GenerateAccesseTokenAsync(user);
            var refresh = await generator.GenerateRefreshTokenAsync();
            return Ok( new
            {

            });
        }


        [Authorize]
        public IActionResult Secret()
        {
            return Content("Very secret information");
        }
    }
}