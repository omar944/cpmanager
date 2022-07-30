namespace API.Models;

public class SimilarUserDto
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? CodeforcesAccount { get; set; }
    public double ProblemsAvg { get; set; }
    public string? Email { get; set; }
    public string? ProfilePhoto { get; set; }
}