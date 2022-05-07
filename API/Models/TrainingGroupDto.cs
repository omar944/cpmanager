namespace API.Models;

public class TrainingGroupDto : BaseDto
{
    public string? Name { get; set; }
    public string? Coach { get; set; }
    public List<string>? Students { get; set; }
}