namespace API.Models
{
    public class UserBlogDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public DateTime? LastActive { get; set; }
        public string? University { get; set; }
        public string? Faculty { get; set; }
        public string? ProfilePhoto { get; set; }
    }
}