namespace API.Models;

public class UsersStatsDto
{
    public int Count { get; set; }
    public int NumOfTeams { get; set; }
    public float PercentageOfNewParticipants { get; set; }
    public float PercentageOfMaleUsers { get; set; }
    public float PercentageOfFemaleUsers { get; set; }
    public int NumStudentsCurrentlyTraining { get; set; }
}