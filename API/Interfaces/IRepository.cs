using API.Data;
using Entities;

namespace API.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    public AppDbContext Context();
    public Task<IEnumerable<TEntity>> GetAllAsync();
    public IQueryable<TEntity> GetQuery();
    public Task<TEntity?> GetByIdAsync(int id);
    public void Add(TEntity entity);
    public void Remove(TEntity entity);
    public void Update(TEntity entity);
    public Task<bool> SaveChangesAsync();
}