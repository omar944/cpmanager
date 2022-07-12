using Entities.App;
namespace API.Models;

public class BlogDto : BaseDto
{
    public string? Content { get; set; }

    public string? Photo { get; set; }

    public UserBlogDto? Author { get; set; }
    
    public DateTime? CreatedAt { get; set; }
}