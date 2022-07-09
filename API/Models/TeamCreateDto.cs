using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class TeamCreateDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public List<int>? Members { get; set; }

    public int? CoachId { get; set; }
}