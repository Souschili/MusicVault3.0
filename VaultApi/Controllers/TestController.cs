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
    /// <summary>
    /// Тестовый контролер
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IOptions<JwtOptions> options;
        private readonly ITokenGenerator generator;
        private readonly DbContext context;
        private readonly IUserManager userManager;
        private readonly IPlayListManager listManager;

        public TestController(IMapper map, IOptions<JwtOptions> opt,
            ITokenGenerator gen, DbContext db, IUserManager user, IPlayListManager play)
        {
            mapper = map;
            options = opt;
            generator = gen;
            context = db;
            userManager = user;
            listManager = play;
        }

        /// <summary>
        /// Тестовый запись плейлиста
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserPlayList")]
        [Authorize]
        public async Task<IActionResult> PlayListsAsync()
        {

            var id = HttpContext.User.FindFirst("ID").Value;

            var rezult = await listManager.Test(id);
            return Ok(rezult);

            // var user = await context.Set<User>().Include(p=> p.PlayLists)
            //     .FirstOrDefaultAsync(x => x.Id.ToString() == userID);
            //
            // return Ok(new
            // {
            //     UserID = userID,
            //     userName = user.Login,
            //     playlists = user.PlayLists
            // });
        }

        /// <summary>
        /// Удаление плейлиста по имени
        /// </summary>
        /// <param name="plName"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync([FromBody]string plName)
        {
            var userID = HttpContext.User.FindFirst("ID").Value;
            var user = await context.Set<User>().Include(p => p.PlayLists)
                .FirstOrDefaultAsync(x => x.Id.ToString() == userID);
            user.PlayLists.RemoveAll(x => x.Name == plName);
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Вывод информации о текущем пользователе
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserInfo")]
        [Authorize]
        public async Task<IActionResult> UserInfoAsync()
        {
            var id = HttpContext.User.FindFirst("ID").Value;
            var user = await context.Set<User>().Include(x=> x.PlayLists)
                .FirstOrDefaultAsync(x => x.Id.ToString() == id);
            return Ok(user);
        }

        [HttpPost("AllUsers")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await context.Set<User>().ToListAsync();
            return Ok(users);
        }
    }
}