namespace Entities.Codeforces;

public class Problem : BaseEntity
{
    public int ContestId { get; set; }
    public string? Index { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public int Rating { get; set; }

    public ICollection<Tag>? Tags { get; set; }
}