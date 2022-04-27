namespace Entities.App;

public class Blog:BaseEntity
{
    public string? Content { get; set; }
    public string? Photo { get; set; }
    public User? Author { get; set; }
}