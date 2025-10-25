using Microsoft.EntityFrameworkCore;
using passwords_backend.Data;
using passwords_backend.Models;

public class AccountHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
{
    private readonly AppDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<ResponseApi<IEnumerable<Account>>> GetAllAccountsAsync(int pageNumber)
    {
        const int pageSize = 10;

        try
        {
            var totalItems = await _context.Accounts.CountAsync();

            var accounts = await _context.Accounts
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
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            Console.WriteLine(errorMessage);
            return new ResponseApi<IEnumerable<Account>>(500, $"Error: {errorMessage}", []);
        }
    }


}