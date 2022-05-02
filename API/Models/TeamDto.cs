namespace API.Models;

public class TeamDto:BaseDto
{
    public string? Name { get; set; }
    public List<string>? Members { get; set; }
}