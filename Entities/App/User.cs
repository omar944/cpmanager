using Entities.Codeforces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Entities.App;

[Index(nameof(FullName))]
public class User : IdentityUser<int>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastActive { get; set; }
    public CodeforcesAccount? CodeforcesAccount { get; set; }
    public string? AtCoderHandle { get; set; }
    public string? CodeChefHandle { get; set; }
    public string? University { get; set; }
    public string? Faculty { get; set; }
    public string? FullName { get; set; }
    public string? ProfilePhoto { get; set; } // TODO: Change this to "UserPhoto" class
    public string? Gender { get; set; }
    public string? Bio { get; set; }

    public ICollection<TeamUser>? Teams { get; set; }
    public ICollection<Team>? Supervising { get; set; }
    public ICollection<Participation>? Participations { get; set; }
    public ICollection<TrainingGroupUser> TrainingGroups { get; set; } = null!;

    public ICollection<TrainingGroup>? TeachingGroups { get; set; }
    public ICollection<Blog>? Blogs { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = null!;
    public ICollection<Submission>? Submissions { get; set; }

    public bool GetIsCoach()
    {
        return UserRoles.Select(x=>x.Role).Any(r=>r.Name=="Coach");
    }
}