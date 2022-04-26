namespace Entities.App;

public class TrainingGroupUser : BaseEntity
{
    public User? User { get; set; }
    public TrainingGroup? TrainingGroup { get; set; }
    public int TrainingGroupId { get; set; }
    public int UserId { get; set; }
}