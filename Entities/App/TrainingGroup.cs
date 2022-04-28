using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.App;

public class TrainingGroup : BaseEntity
{
    public string? Name { get; set; }
    public ICollection<TrainingGroupUser>? Students { get; set; }
    public User Coach { get; set; } = null!;
    public int CoachId { get; set; }
    
    [NotMapped]
    public ICollection<DateTime>? TrainingTimes { get; set; }
}