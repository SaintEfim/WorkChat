using System.ComponentModel.DataAnnotations;

namespace Service.Authify.API.Models.RequestsDto;

/// <summary>
/// Represents the data transfer object for login requests.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// The email address of the user.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = String.Empty;

    /// <summary>
    /// The password of the user.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = String.Empty;
}