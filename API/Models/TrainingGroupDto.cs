namespace API.Models;

public class TrainingGroupDto : BaseDto
{
    public string? Name { get; set; }
    public TeamUserDto? Coach { get; set; }
    public List<int>? Students { get; set; }
    public string? Level { get; set; }
}