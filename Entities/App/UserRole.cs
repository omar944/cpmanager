using Microsoft.AspNetCore.Identity;

namespace Entities.App;

public class UserRole : IdentityUserRole<int>
{
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}