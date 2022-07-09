namespace Entities.App;

public class TeamUser : BaseEntity
{
    public User? User { get; init; }
    public Team? Team { get; init; }
    public int TeamId { get; set; }
    public int UserId { get; set; }
}