using Entities.App;

namespace API.Models;

public class UserDto
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
    
    public ICollection<TeamDto>? Teams { get; set; }
    public ICollection<ParticipationDto>? Participations { get; set; }
    public ICollection<TrainingGroupDto>? TrainingGroups { get; set; }
    public ICollection<TrainingGroupDto>? TeachingGroups { get; set; }
    public bool IsCoach { get; set; }
}