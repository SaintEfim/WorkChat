using System.ComponentModel.DataAnnotations;

namespace Service.Authify.API.Models;

/// <summary>
///     The model to be used for an user update.
/// </summary>
public class UserCredentialUpdateDto
{
    /// <summary>
    ///     The password of the user.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}