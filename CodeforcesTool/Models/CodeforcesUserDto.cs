namespace CodeforcesTool.Models;

public class CodeforcesUserDto
{
    public string? Handle { get; set; }
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Organization { get; set; }
    
    public string? MaxRank { get; set; }
    public string? Rank { get; set; }
    public int Rating { get; set; }
    public int MaxRating { get; set; }
    
    public int LastOnlineTimeSeconds { get; set; }
    public int RegistrationTimeSeconds { get; set; }
    
    public string? TitlePhoto { get; set; }
    public string? Avatar { get; set; }
    
    public int FriendOfCount { get; set; }
    public int Contribution { get; set; }

    public string? Email { get; set; }
}