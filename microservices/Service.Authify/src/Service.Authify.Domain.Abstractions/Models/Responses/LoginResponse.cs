namespace Service.Authify.Domain.Models.Responses;

public class LoginResponse
{
    public string TokenType { get; set; } = String.Empty;
    public string AccessToken { get; set; } = String.Empty;
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; } = String.Empty;
}