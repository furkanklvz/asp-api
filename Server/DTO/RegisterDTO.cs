using System.ComponentModel.DataAnnotations;

namespace Server.DTO;

public class RegisterDTO
{
    [Required]
    public string UserName { get; set; } = null!;
    public string? FullName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

}