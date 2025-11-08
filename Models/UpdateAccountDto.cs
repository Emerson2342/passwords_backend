namespace passwords_backend.Models;

public class UpdateAccountDto
{
    public string? AccountName { get; set; }
    public string? PasswordHash { get; set; }
    public bool? IsOnTrash { get; set; }
}
