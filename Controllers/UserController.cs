using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using passwords_backend.Models;

namespace passwords_backend.Controllers
{
    [ApiController]
    [Route("v1/api/[controller]")]
    public class UserController(UserHandler userHandler) : ControllerBase
    {
        private readonly UserHandler _userHandler = userHandler;

        [HttpPost("login")]
        public async Task<ResponseApi<string>> Login([FromBody] LoginDTO login)
        {
            return await _userHandler.Login(login);
        }

        [HttpPost("create")]
        public async Task<ResponseApi<string>> CreateUser([FromBody] CreateUserDTO dto)
        {
            return await _userHandler.CreateUser(dto);
        }



    }
}