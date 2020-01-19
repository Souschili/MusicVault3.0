using System;
using System.Collections.Generic;
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
        [Authorize]
        [HttpPost("UploadMusic")]
        public IActionResult UploadMusic()
        {
            return Ok("Secret content");
        }

    }
}