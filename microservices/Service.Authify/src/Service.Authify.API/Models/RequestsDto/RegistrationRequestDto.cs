﻿using System.ComponentModel.DataAnnotations;

namespace Service.Authify.API.Models.RequestsDto;

/// <summary>
/// The person responsible for some object processing or the one who is currently executing some task.
/// </summary>
public class RegistrationRequestDto
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
}