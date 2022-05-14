namespace API.Models;

public class DailyTaskDto:BaseDto
{
    public List<ProblemDto>? Problems { get; set; }
    public DateTime? DueDate { get; set; }
    public TrainingGroupDto? Group { get; set; }
    public string? Description { get; set; }
}