using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicVault.Data.Entity;
using MusicVault.Services.Interfaces;

namespace VaultApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayListController : ControllerBase
    {
        //пока напрямую
        private readonly IPlayListManager listManager;
        public PlayListController(IPlayListManager playList)
        {
            listManager = playList;
        }


        [HttpPost("CreatePlayList")]
        [Authorize]
        public IActionResult Create([FromBody]string name)
        {
            // Если по каким то причина имя пустое 
            if (String.IsNullOrWhiteSpace(name)) return BadRequest(new { error = "Name Can't null or whitespace" });
            //получаем из контекста запроса клайм хранящий айди
            var userId = HttpContext.User.FindFirst("ID").Value;

            listManager.CreatePlayListAsynс(name, userId);

            return Ok("PlayList Added");
        }
    }
}