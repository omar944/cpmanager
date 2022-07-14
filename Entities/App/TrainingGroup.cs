namespace Entities.App;

public class TrainingGroup : BaseEntity
{
    public string? Level { get; set; }
    public string? Name { get; set; }
    public List<TrainingGroupUser>? Students { get; set; }
    public User Coach { get; set; } = null!;
    public int CoachId { get; set; }
}