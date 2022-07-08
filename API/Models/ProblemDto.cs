namespace API.Models;

public class ProblemDto
{
    public string? Name { get; set; }
    public string? Url { get; set; }
    public int Rating { get; set; }
    public List<string>? Tags { get; set; }
    public string? Index { get; set; }
    public int ContestId { get; set; }
}