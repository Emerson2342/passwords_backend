using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using passwords_backend.Data;
using passwords_backend.Handlers;
using passwords_backend.Models;

namespace passwords_backend.Controllers
{
    [ApiController]
    [Route("v1/api/[controller]")]
    [Authorize]
    public class AccountController(AccountHandler accountHandler) : ControllerBase
    {
        private readonly AccountHandler _accountHandler = accountHandler;

        [HttpGet("all")]
        public async Task<ResponseApi<IEnumerable<Account>>> GetAllAccounts([FromQuery] int pageNumber = 1)
        {
            return await _accountHandler.GetAllAccountsAsync(pageNumber);
        }

        [HttpGet("{id}")]
        public async Task<ResponseApi<Account>> GetAccount(Guid id)
        {
            return await _accountHandler.GetAccountAsync(id);
        }

        [HttpPut("{id}")]
        public async Task<ResponseApi<string>> UpdateAccount(Guid id, [FromBody] Account updateAccount)
        {

            return await _accountHandler.UpdateAccountAsync(id, updateAccount);
        }


        [HttpPost]
        public async Task<ResponseApi<string>> AddAccount([FromBody] Account account)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                string errorMessage = string.Join("; ", errors);

                return new ResponseApi<string>(504, errorMessage, null);
            }
            return await _accountHandler.AddAccountAsync(account);
        }

        [HttpDelete("{id}")]
        public async Task<ResponseApi<string>> DeleteAccount(Guid id)
        {
            return await _accountHandler.DeleteAccount(id);
        }

    }
}