using System.ComponentModel.DataAnnotations;

namespace Service.Authify.API.Models;

/// <summary>
/// The person responsible for some object processing or the one who is currently executing some task.
/// </summary>
public class UserCredentialDto
{
    /// <inheridoc/>
    [Required]
    public Guid Id { get; set; }

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

    /// <summary>
    /// The role of the user.
    /// </summary>
    public string Role { get; set; } = String.Empty;

    /// <summary>
    /// The date and time when the user credentials were created.
    /// </summary>
    [Required(ErrorMessage = "CreatedAt is required.")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The last update date and time of the user credentials.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}