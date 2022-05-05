namespace Entities.App;

public abstract class Photo : BaseEntity
{
    public virtual string? Url { get; set; }
    
    public virtual string? PublicId { get; set; }
}