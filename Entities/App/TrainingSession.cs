namespace Entities.App;

public class TrainingSession:BaseEntity
{
    public DateTime? SessionDate { get; set; }
    public string? Description { get; set; }
}