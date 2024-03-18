using System.ComponentModel.DataAnnotations;

namespace Service.Authify.API.Models.RequestsDto;

public class ResetPasswordDto
{
    /// <summary>
    /// The email address of the user.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The password of the user.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// The new password of the user.
    /// </summary>
    [Required(ErrorMessage = "NewPassword is required.")]
    public string NewPassword { get; set; } = string.Empty;
}