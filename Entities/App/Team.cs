using Microsoft.EntityFrameworkCore;

namespace Entities.App;

[Index(nameof(Name), Name = "Index_Team_Name")]
public class Team : BaseEntity
{
    public string? Name { get; set; }
    public ICollection<TeamUser>? Members { get; set; }
    public User? Coach { get; set; }
    public int CoachId { get; set; }
}