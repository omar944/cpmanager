namespace API.Models;

public class BlogCreateDto
{
    public string? Content { get; set; }
    public IFormFile? Image { get; set; }
}