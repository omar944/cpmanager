namespace Entities.App;

public class Team : BaseEntity
{
    public string? Name { get; set; }
    public ICollection<User>? Members { get; set; }
}