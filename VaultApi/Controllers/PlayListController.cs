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
    /// <summary>
    /// Плейлисты
    /// </summary>
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

        /// <summary>
        /// Удалить плейлист
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("DeletePlayList")]
        [Authorize]
        public async Task<IActionResult> DeletePlayListAsync([FromBody]string name)
        {
            var userId = HttpContext.User.FindFirst("ID").Value;
            await listManager.DeletePlayListAsync(name, userId);
            return Ok();
        }

        /// <summary>
        /// Создать плейлист
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("CreatePlayList")]
        [Authorize]
        public async Task<IActionResult> CreatePlayListAsync([FromBody]string name)
        {
            // Если по каким то причина имя пустое 
            if (String.IsNullOrWhiteSpace(name)) return BadRequest(new { error = "Name Can't null or whitespace" });
            //получаем из контекста запроса клайм хранящий айди
            var userId = HttpContext.User.FindFirst("ID").Value;
            try
            {
               await listManager.CreatePlayListAsynс(name, userId);
               return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            
        }

        /// <summary>
        /// Получить все плейлисты
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetAllPlayList")]
        [Authorize]
        public async Task<IActionResult> GetAllPlayListAsync()
        {
            var ownerId = HttpContext.User.FindFirst("ID").Value;
            var rezult = await listManager.GetAllPlayListAsync(ownerId);
            return Ok(rezult);
        }
    }
}