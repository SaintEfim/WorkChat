using System.ComponentModel.DataAnnotations;

namespace Service.Authify.API.Models.ResponsesDto;

/// <summary>
/// The person responsible for some object processing or the one who is currently executing some task.
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// The type of token.
    /// </summary>
    [Required(ErrorMessage = "TokenType is required.")]
    public string TokenType { get; set; } = string.Empty;

    /// <summary>
    /// The access token.
    /// </summary>
    [Required(ErrorMessage = "AccessToken is required.")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// The expiration time of the access token in seconds.
    /// </summary>
    [Required(ErrorMessage = "ExpiresIn is required.")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// The refresh token.
    /// </summary>
    [Required(ErrorMessage = "RefreshToken is required.")]
    public string RefreshToken { get; set; } = string.Empty;
}