namespace Entities.Codeforces;

public class Tag : BaseEntity
{
    public string? Name { get; set; }
    public ICollection<Problem>? Problems { get; set; }
}