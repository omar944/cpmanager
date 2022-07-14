using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class TrainingSessionDto: BaseDto
{
    public DateTime? SessionDate { get; set; }
    public string? Description { get; set; }
}