namespace API.Models;

public class TeamDto : BaseDto
{
    public string? Name { get; set; }
    public List<int>? Members { get; set; }
    public TeamUserDto? Coach { get; set; }
}

public class TeamUserDto
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastActive { get; set; }
    public string? CodeforcesAccount { get; set; }
    public string? AtCoderHandle { get; set; }
    public string? CodeChefHandle { get; set; }
    public string? University { get; set; }
    public string? Faculty { get; set; }
    public string? ProfilePhoto { get; set; }
    public string? Gender { get; set; }
}