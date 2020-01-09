using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicVault.Data.Entity;
using VaultApi.ViewModels;

namespace VaultApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMapper mapper;
        public TestController(IMapper map)
        {
            mapper = map;
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
    }
}