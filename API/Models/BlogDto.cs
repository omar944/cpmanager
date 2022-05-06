using Entities.App;
namespace API.Models;

public class BlogDto : BaseDto
{
    public string? Content { get; set; }
    
    public string? Photo { get; set; }
    
    public User? Author { get; set; }
}