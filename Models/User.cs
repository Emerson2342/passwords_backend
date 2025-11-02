using passwords_backend.Services;

namespace passwords_backend.Models
{

    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<Account> Accounts { get; set; } = [];



        public static User CreateUser(CreateUserDTO user)
        {
            var passwordHash = PasswordService.HashPassword(user.Password);
            return new User
            {
                Email = user.Email,
                UserName = user.UserName,
                PasswordHash = passwordHash
            };
        }




    }
}