namespace API.Models;

public class BlogUpdateDto
{
    public string? Content { get; set; }
    public IFormFile? Image { get; set; }
}