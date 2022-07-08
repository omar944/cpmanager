namespace API.Models;

public class TrainingGroupDto : BaseDto
{
    public string? Name { get; set; }
    public int Coach { get; set; }
    public List<int>? Students { get; set; }
}