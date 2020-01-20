using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicVault.Data.Entity;

namespace VaultApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayListController : ControllerBase
    {
        //пока напрямую
        private readonly DbContext context;
        public PlayListController(DbContext db)
        {
            context = db;
        }


        [HttpPost("CreatePlayList")]
        [Authorize]
        public IActionResult Create([FromBody]string name)
        {
            if (String.IsNullOrWhiteSpace(name)) return BadRequest(new { error = "Name Can't null or whitespace" });


            var pl = new PlayList { Name = name, UserId = HttpContext.User.FindFirst("ID").Value };
            context.Set<PlayList>().Add(pl);
            context.SaveChanges();
            return Ok("PlayList Added");
        }
    }
}