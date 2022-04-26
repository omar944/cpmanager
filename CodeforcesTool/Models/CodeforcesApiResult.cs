namespace CodeforcesTool.Models;

public class CodeforcesApiResult<T>
{
    public string? Status { get; set; }
    public string? Comment { get; set; }
    public T? Result { get; set; }
}