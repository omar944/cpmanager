namespace Entities.App;

public class TrainingGroupUser : BaseEntity
{
    public User User { get; init; } = null!;
    public TrainingGroup TrainingGroup { get; init; } = null!;
    public int TrainingGroupId { get; set; }
    public int UserId { get; set; }
}