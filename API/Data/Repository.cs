using Entities;
using API.Interfaces;

namespace API.Data;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<TEntity> _entities;
    public Repository(AppDbContext context)
    {
        _context = context;
        _entities = _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public IQueryable<TEntity> GetQuery()
    {
        return _entities.AsQueryable();
    }

    public async Task<TEntity?> GetAsync(int id)
    {
        return await _entities.SingleOrDefaultAsync(e => e.Id == id);
    }

    public void Add(TEntity entity)
    {
        _entities.Add(entity);
    }

    public void Remove(TEntity entity)
    {
        _entities.Remove(entity);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}