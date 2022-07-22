namespace API.Models;

public class UserTaskStatsDto : BaseDto
{
    public List<ProblemDto>? Problems { get; set; }
    public DateTime? DueDate { get; set; }
    public TrainingGroupDto? Group { get; set; }
    public string? Description { get; set; }
    
}