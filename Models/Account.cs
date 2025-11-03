using System.ComponentModel.DataAnnotations;

namespace passwords_backend.Models
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Nome é um campo obrigatório")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é um campo obrigatório")]
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsOnTrash { get; set; } = false;

        [Required]
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}