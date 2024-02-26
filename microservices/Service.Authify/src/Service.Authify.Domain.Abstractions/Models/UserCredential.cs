namespace Service.Authify.Domain.Models;

public class UserCredential
{
    public UserCredential()
    {
        Role = "user";
    }
    public Guid Id { get; set; }
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string Role { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}