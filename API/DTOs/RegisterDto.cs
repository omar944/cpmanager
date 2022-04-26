using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required] 
    public string? UserName { get; set; }

    [StringLength(16, MinimumLength = 5)]
    [Required]
    public string? Password { get; set; }
}