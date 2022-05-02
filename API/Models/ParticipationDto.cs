namespace API.Models;

public class ParticipationDto
{
    public int Rank { get; set; }
    public string? Year { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    
    public TeamDto? Team { get; set; }
}