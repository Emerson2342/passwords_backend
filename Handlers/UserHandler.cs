using Microsoft.EntityFrameworkCore;
using passwords_backend.Data;
using passwords_backend.Models;
using passwords_backend.Services;

public class UserHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<AccountHandler> _logger;

    private readonly string textError = "Ocorreu um erro, por favor, tente novamente mais tarde";

    private readonly ResponseApi<string> errorResponse;



    public UserHandler(AppDbContext context, ILogger<AccountHandler> logger)
    {
        _context = context;
        _logger = logger;
        errorResponse = new ResponseApi<string>(500, textError, null);
    }
    public async Task<ResponseApi<string>> Login(LoginDTO login)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == login.UserName);

            if (user != null)
            {
                var passwordHash = PasswordService.VerifyPassword(login.Password, user.PasswordHash);
                if (passwordHash)
                    return new ResponseApi<string>(200, "Success", null);
                return new ResponseApi<string>(404, "Senha e/ou usuário não encontrado!", null);
            }
            else
            {
                return new ResponseApi<string>(404, "Senha e/ou usuário não encontrado!", null);
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login");
            return errorResponse;
        }
    }

    public async Task<ResponseApi<string>> CreateUser(CreateUserDTO newUser)
    {
        try
        {

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário");
            return errorResponse;
        }
    }

}