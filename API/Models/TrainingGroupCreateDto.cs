using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class TrainingGroupCreateDto
{
    public string? Name { get; set; }
    [Required]
    public List<int>? Students { get; set; }
    public string? Level { get; set; }
}