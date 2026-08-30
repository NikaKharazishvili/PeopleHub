using System.ComponentModel.DataAnnotations;

namespace PeopleHub.DTOs;

public class RegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } = "";
}