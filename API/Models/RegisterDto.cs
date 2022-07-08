using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class RegisterDto
{
    [Required] 
    public string? FullName { get; set; }
    
    [Required]
    [EmailAddress(ErrorMessage = "not a valid email")]
    public string? Email { get; set; }
    
    [StringLength(16, MinimumLength = 5)]
    [Required]
    public string? Password { get; set; }

    public string? Gender { get; set;}

    public string? Status { get; set; }
}