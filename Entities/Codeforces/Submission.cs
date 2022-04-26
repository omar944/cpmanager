using Entities.App;

namespace Entities.Codeforces;

public class Submission : BaseEntity
{
    // public int ContestId { get; set; }
    // public int CreationTimeSeconds { get; set; }
    // public long RelativeTimeSeconds { get; set; }
    // public int TimeConsumedMillis { get; set; }
    // public int MemoryConsumedBytes { get; set; }
    public Problem? Problem { get; set; }
    public string? ProgrammingLanguage { get; set; }
    public string? Verdict { get; set; }
    public User? Author { get; set; }
}