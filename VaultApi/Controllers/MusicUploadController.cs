using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VaultApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicUploadController : ControllerBase
    {
        
        [HttpPost("UploadMusic")]
        public IActionResult UploadMusic(IFormFile uploaded)
        {
            var str=Path.GetExtension(uploaded.FileName);
            if(uploaded!=null)
            {
                return Ok(new { FileName = uploaded.FileName,FileLenth=uploaded.Length ,Extension=str});
            }
            return Ok("");
        }

    }
}