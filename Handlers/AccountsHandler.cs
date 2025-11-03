using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using passwords_backend.Data;
using passwords_backend.Models;

public class AccountHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<AccountHandler> logger)
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<AccountHandler> _logger = logger;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private readonly string textError = "Ocorreu um erro, por favor, tente novamente mais tarde";

    public async Task<ResponseApi<IEnumerable<Account>>> GetAllAccountsAsync(int pageNumber)
    {
        var pageSize = 10;
        try
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var totalItems = await _context.Accounts.CountAsync();

            var accounts = await _context.Accounts
            .Where(x => x.UserId == userId)
            .OrderBy(a => a.AccountName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(10)
            .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var paginationMetadata = new { totalItems, pageNumber, pageSize, totalPages };

            var response = _httpContextAccessor.HttpContext?.Response;
            response?.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return new ResponseApi<IEnumerable<Account>>(200, "Success", accounts);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar as senhas");
            return new ResponseApi<IEnumerable<Account>>(500, textError, null);
        }
    }

    public async Task<ResponseApi<Account>> GetAccountAsync(Guid id)
    {
        try
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return new ResponseApi<Account>(404, "Not found", null);
            }
            else
            {
                return new ResponseApi<Account>(200, "Success", account);
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar a conta do Id {id}", id);
            return new ResponseApi<Account>(500, textError, null);
        }
    }
    public async Task<ResponseApi<string>> AddAccountAsync(Account newAccount)
    {
        try
        {
            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();
            return new ResponseApi<string>(200, "Success", null);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao adicionar a conta {newAccount.AccountName}", newAccount.Id);
            return new ResponseApi<string>(500, textError, null);
        }
    }

    public async Task<ResponseApi<string>> UpdateAccountAsync(Guid id, Account updateAccount)
    {
        try
        {
            var currentAccount = await _context.Accounts.FindAsync(id);
            if (currentAccount == null)
            {
                return new ResponseApi<string>(404, "Not found!", null);
            }
            _context.Accounts.Entry(currentAccount).CurrentValues.SetValues(updateAccount);
            await _context.SaveChangesAsync();
            return new ResponseApi<string>(200, "Success", null);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao atualizar a conta com ID {id}", id);
            return new ResponseApi<string>(500, textError, null);
        }
    }

    public async Task<ResponseApi<string>> DeleteAccount(Guid id)
    {
        try
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return new ResponseApi<string>(404, "Conta não encontrada!", null);
            }
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return new ResponseApi<string>(200, "Success", $"{account.AccountName} accounts was deleted!");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao deletar conta com ID {id}", id);
            return new ResponseApi<string>(500, textError, null);
        }
    }


}