using API.Data;
using API.Models;
using Entities;

namespace API.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    public AppDbContext Context();
    public Task<IEnumerable<TEntity>> GetAllAsync();
    public IQueryable<TEntity> GetQuery();
    public Task<TEntity?> GetByIdAsync(int id);
    public Task<IEnumerable<TDto>> GetProjected<TDto>() where TDto : BaseDto;
    public Task<TDto?> GetProjectedById<TDto>(int id) where TDto : BaseDto;
    public void Add(TEntity entity);
    public void Remove(TEntity entity);
    public void Update(TEntity entity);
    public Task<bool> SaveChangesAsync();
}