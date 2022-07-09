namespace Entities.App;

public class Team : BaseEntity
{
    public string? Name { get; set; }
    public ICollection<TeamUser>? Members { get; set; }
    public User? Coach { get; set; }
    public int CoachId { get; set; }
}