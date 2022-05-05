namespace Entities.App;

public class Blog:BaseEntity
{
    public string? Content { get; set; }
    public string? PhotoUrl { get; set; }
    public User Author { get; set; } = null!;
}