using Microsoft.AspNetCore.Identity;

namespace Entities.App;

public class UserRole:IdentityUserRole<int>
{
    public User User { get; set; }
    public Role Role { get; set; }
}