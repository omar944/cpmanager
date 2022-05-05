using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.App;

[Table("BlogPhotos")]
public class BlogPhoto : Photo
{
    public Blog? Blog { get; set; }

    public int BlogId { get; set; }
}