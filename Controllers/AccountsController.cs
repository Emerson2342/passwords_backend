using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using passwords_backend.Data;
using passwords_backend.Models;

namespace passwords_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly AccountHandler _accountHandler;

        public AccountsController(AppDbContext appDbContext, AccountHandler accountHandler)
        {
            _appDbContext = appDbContext;
            _accountHandler = accountHandler;
        }
        [HttpGet]
        public async Task<ResponseApi<IEnumerable<Account>>> GetAllAccounts([FromQuery] int pageNumber = 1)
        {
            return await _accountHandler.GetAllAccountsAsync(pageNumber);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(Guid id)
        {
            var account = await _appDbContext.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound("Conta não encontrada");
            }

            return Ok(account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] Account updateAccount)
        {
            var actualAccount = await _appDbContext.Accounts.FindAsync(id);

            if (actualAccount == null)
            {
                return NotFound("Conta não encontrada!");
            }

            _appDbContext.Entry(actualAccount).CurrentValues.SetValues(updateAccount);
            await _appDbContext.SaveChangesAsync();
            return StatusCode(201, updateAccount);
        }


        [HttpPost]
        public async Task<IActionResult> AddAccount([FromBody] Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _appDbContext.Accounts.Add(account);
            await _appDbContext.SaveChangesAsync();
            return Created("Senha salva com sucesso!", account);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            var account = await _appDbContext.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound("Conta não encontrada!");
            }

            _appDbContext.Accounts.Remove(account);
            await _appDbContext.SaveChangesAsync();
            return Ok("Conta deletada com sucesso!");
        }

    }
}