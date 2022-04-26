namespace Entities.App;

public class TrainingGroup : BaseEntity
{
    public string? Name { get; set; }
    public ICollection<TrainingGroupUser>? Students { get; set; }
    public User? Coach { get; set; }
    public int CoachId { get; set; }
}