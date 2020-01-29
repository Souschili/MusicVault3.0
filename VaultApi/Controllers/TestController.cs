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



    }
}