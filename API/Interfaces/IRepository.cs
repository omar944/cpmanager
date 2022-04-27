using Entities;

namespace API.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    public Task<IEnumerable<TEntity>> GetAllAsync();
    public IQueryable<TEntity> GetQuery();
    public Task<TEntity?> GetAsync(int id);
    public void Add(TEntity entity);
    public void Remove(TEntity entity);
    public Task<bool> SaveChangesAsync();
}