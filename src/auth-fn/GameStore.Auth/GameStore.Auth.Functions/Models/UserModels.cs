using System.ComponentModel.DataAnnotations;

namespace GameStore.Auth.Functions.Models;

/// <summary>
/// Stored user. Serialized with PascalCase names, as in the original encrypted file.
/// </summary>
public sealed class User
{
    public string Email { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

public sealed record UserResponse(string Email, string FirstName, string LastName)
{
    public static UserResponse From(User user) => new(user.Email, user.FirstName, user.LastName);
}

public sealed class AuthRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}

public sealed class UserChangeRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    public string LastName { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; init; } = string.Empty;

    public User ToUser() => new()
    {
        Email = Email,
        FirstName = FirstName,
        LastName = LastName,
        Password = Password,
    };
}
