namespace Service.Authify.Domain.Models.Requests;

public class RegistrationRequest
{
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string Role { get; set; } = String.Empty;
}