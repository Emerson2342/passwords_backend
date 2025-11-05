using Microsoft.EntityFrameworkCore;
using passwords_backend.Data;
using passwords_backend.Handlers;
using passwords_backend.Models;
using passwords_backend.Services;

public class UserHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<AccountHandler> _logger;
    private readonly TokenService _tokenService;

    private readonly string textError = "Ocorreu um erro, por favor, tente novamente mais tarde";

    private readonly ResponseApi<string> errorResponse;



    public UserHandler(AppDbContext context, ILogger<AccountHandler> logger, TokenService tokenService)
    {
        _context = context;
        _logger = logger;
        errorResponse = new ResponseApi<string>(500, textError, null);
        _tokenService = tokenService;
    }
    public async Task<ResponseApi<string>> Login(LoginDTO login)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == login.Email);

            if (user != null)
            {
                var passwordHash = PasswordService.VerifyPassword(login.Password, user.PasswordHash);
                if (passwordHash)
                {
                    var token = _tokenService.CreateToken(user);
                    return new ResponseApi<string>(200, "Success", token);
                }
                return new ResponseApi<string>(404, "Senha e/ou usuário inválido(s)!", null);
            }
            else
            {
                return new ResponseApi<string>(404, "Senha e/ou usuário inválido(s)!", null);
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
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);

            if (existingUser != null)
            {
                return new ResponseApi<string>(400, "Email já cadastrado", null);
            }

            var user = User.CreateUser(newUser);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new ResponseApi<string>(200, "Usuário criado com sucesso", null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário");
            return errorResponse;
        }
    }

}