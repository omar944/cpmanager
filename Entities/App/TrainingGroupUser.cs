namespace Entities.App;

public class TrainingGroupUser : BaseEntity
{
    public User? User { get; init; }
    public TrainingGroup? TrainingGroup { get; init; }
    public int TrainingGroupId { get; set; }
    public int UserId { get; set; }
}