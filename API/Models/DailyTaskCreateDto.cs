using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class DailyTaskCreateDto
{
    public List<string>? Problems { get; set; }
    public DateTime? DueDate { get; set; }
    [Required]
    public int GroupId { get; set; }
    public string? Description { get; set; }
}