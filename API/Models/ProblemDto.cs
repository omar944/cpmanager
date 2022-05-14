namespace API.Models;

public class ProblemDto
{
    public string? Name { get; set; }
    public string? Link { get; set; }
    public int Rating { get; set; }
    public List<string>? Tags { get; set; }
}