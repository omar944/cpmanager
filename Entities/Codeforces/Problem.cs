using Entities.App;

namespace Entities.Codeforces;

public class Problem : BaseEntity
{
    public int ContestId { get; set; }
    public string? Index { get; set; }
    public string? Name { get; set; }
    public int Rating { get; set; }

    public ICollection<Tag>? Tags { get; set; }
    public ICollection<DailyTask>? Tasks  { get; set; }
    public ICollection<Submission>? Submissions { get; set; }
    
    public string GetLink()
    {
        return $"https://codeforces.com/contest/{ContestId}/problem/{Index}";
    }
}