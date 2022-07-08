namespace Entities.Codeforces;

public class Tag : BaseEntity
{
    public string? Name { get; set; }
    public ICollection<Problem>? Problems { get; set; }
}

public static class UsedTags
{
    public static List<string> TagsUsed { get; } = new()
    {
        "binary search",
        "brute force",
        "constructive algorithms",
        "data structures",
        "dfs and similar",
        "dp",
        "graphs",
        "greedy",
        "implementation",
        "interactive",
        "math",
        "strings",
        "sortings"
    };
}