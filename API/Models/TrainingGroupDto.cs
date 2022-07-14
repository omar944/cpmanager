namespace API.Models;

public class TrainingGroupDto : BaseDto
{
    public string? Level { get; set; }
    public string? Name { get; set; }
    public List<TrainingGroupStudentDto>? Students { get; set; }
    public TrainingGroupStudentDto? Coach { get; set; }
}

public class TrainingGroupStudentDto
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? ProfilePhoto { get; set; }
}