namespace Entities.App;

public class UserPhoto : Photo
{
    public User? User { get; set; }

    public int? UserId { get; set; }
    
}