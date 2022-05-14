namespace Entities;

public abstract class BaseEntity
{
    public virtual int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}