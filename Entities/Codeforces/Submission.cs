using Entities.App;

namespace Entities.Codeforces;

public class Submission : BaseEntity
{
    public string? Verdict { get; set; }

    public Problem? Problem { get; set; }
    public int ProblemId { get; set; }
    public User? Author { get; set; }
    public int UserId { get; set; }
    
}