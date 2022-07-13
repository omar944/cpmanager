namespace API.Models;

public class UserUpdateDto
{
    public string? AtCoderHandle { get; set; }
    public string? CodeChefHandle { get; set; }
    public string? University { get; set; }
    public string? Faculty { get; set; }
    public string? FullName { get; set; }
    public string? Bio { get; set; }
}