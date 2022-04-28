using Microsoft.AspNetCore.Identity;

namespace Entities.App;

public class Role : IdentityRole<int>
{
    public ICollection<UserRole> UserRoles { get; set; } = null!;
}