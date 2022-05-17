using Entities.Codeforces;
using Microsoft.AspNetCore.Identity;

namespace Entities.App;

public class User : IdentityUser<int>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastActive { get; set; }
    public CodeforcesAccount? CodeforcesAccount { get; set; }
    public string? AtCoderHandle { get; set; }
    public string? CodeChefHandle { get; set; }
    public string? University { get; set; }
    public string? Faculty { get; set; }
    public string? ProfilePhoto { get; set; } // TODO: Change this to "UserPhoto" class

    public ICollection<Team>? Teams { get; set; }
    public ICollection<Participation>? Participations { get; set; }
    public ICollection<TrainingGroupUser> TrainingGroups { get; set; } = null;

    public ICollection<TrainingGroup>? TeachingGroups { get; set; }
    public ICollection<Blog>? Blogs { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = null!;

    public ICollection<Submission>? Submissions { get; set; }
    
}