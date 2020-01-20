using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicVault.Data.Entity;
using MusicVault.Services.Helpers;
using MusicVault.Services.Interfaces;
using System;
using System.Linq;
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
        private readonly DbContext context;
        private readonly IUserManager userManager;

        public TestController(IMapper map,IOptions<JwtOptions> opt,ITokenGenerator gen,DbContext db,IUserManager user)
        {
            mapper = map;
            options = opt;
            generator = gen;
            context = db;
            userManager = user;
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

        [HttpGet("Token")]
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
                access=token,
                refresh=refresh

            });
        }

        [HttpPost("AddToken")]
        public async Task<IActionResult> AddTokenAsync()
        {
            var user = await context.Set<User>().FirstOrDefaultAsync();
            if (user == null) return BadRequest("No any users in DB");

            var token = new RefreshToken { Content = "this is string token " };
            user.Tokens.Add(token);
            context.SaveChanges();
            return Ok("Token Added");

        }

        [HttpGet("User")]
        public async Task<IActionResult> GetUserAsync()
        {
            return Ok(await context.Set<RefreshToken>().ToListAsync());
        }
        [HttpGet("Secret")]
        [Authorize]
        public IActionResult Secret()
        {
            return Content("Very secret information");
        }


        [HttpGet("GetId")]
        [Authorize]
        public IActionResult Id()
        {
            var id = HttpContext.User.Claims.ToList();
            return Ok(HttpContext.User.FindFirst("ID").Value); 
            //return Ok(new { UserID = id[0].Value });

            #region памятка по httpcontext клаймы
            //return Ok(new
            //{
            //    // {
            //    // "claim1": "5db105c6-bb97-49ff-e630-08d79dac47f6",
            //    // "claim2": "user@example.com",
            //    // "claim3": "demo",
            //    // "claim4": "1579527080",
            //    // "claim5": "1579635080"
            //    // }
            //    // тестовый вывод клаймов
            //    UserAccesseToken=HttpContext.Request.Headers["Autorization"],
            //    ClaimsCount = id.Count,
            //    UserId = id[0].Value,
            //    UserEmail = id[1].Value,
            //    UserLogin = id[2].Value,
            //    nbf = id[3].Value,
            //    exp = id[4].Value,
            //    ExpAfterSecoond = Int32.Parse(id[4].Value) - Int32.Parse(id[3].Value),
            //    Claim5=id[5].Value,
            //    Claim6=id[6].Value
            //});
            #endregion
        }


        [HttpPost("AddPlaylist")]
        public IActionResult AddPlaylist([FromBody]string playlistName)
        {
            var userId = HttpContext.User.Claims.ToList();
            context.Set<PlayList>().AddAsync(new PlayList { Name = playlistName,UserId=userId[0].Value });
            context.SaveChanges();
            //var userInfo=context.Set<User>().
            return Ok();
        }

    }
}