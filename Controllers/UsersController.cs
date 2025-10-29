using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using passwords_backend.Models;

namespace passwords_backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserHandler _userHandler;

        public UsersController(UserHandler userHandler)
        {
            _userHandler = userHandler;
        }

        public async Task<ResponseApi<string>> Login([FromBody] LoginDTO login)
        {
            return await _userHandler.Login(login);
        }



    }
}