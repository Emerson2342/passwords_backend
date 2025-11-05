using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace passwords_backend.Models
{
    public class Account
    {
        [Key]
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Nome é um campo obrigatório")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha é um campo obrigatório")]
        public string PasswordHash { get; set; } = string.Empty;

        [JsonIgnore]
        public bool IsOnTrash { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}