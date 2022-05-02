namespace API.Models;

public class TrainingGroupDto
{
    public string? Name { get; set; }
    public string? Coach { get; set; }
    public List<string>? Students { get; set; }
}