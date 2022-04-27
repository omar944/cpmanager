using Entities.Codeforces;

namespace Entities.App;

public class DailyTask:BaseEntity
{
    public ICollection<Problem>? Problem { get; set; }
    public DateTime? DueDate { get; set; }
    public TrainingGroup? Group { get; set; }
    public string? Description { get; set; }
}