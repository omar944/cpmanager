using Entities;
using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;

namespace API.Data;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly DbSet<TEntity> _entities;

    public Repository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _entities = _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _entities.ToListAsync();
    

    public IQueryable<TEntity> GetQuery()
        => _entities.AsQueryable();

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _entities.SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<TDto>> GetProjected<TDto>() where TDto : BaseDto
    {
        return await _entities.ProjectTo<TDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<TDto?> GetProjectedById<TDto>(int id)where TDto:BaseDto
    {
        return await _entities.
            ProjectTo<TDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Add(TEntity entity)
    {
        _entities.Add(entity);
    }

    public void Remove(TEntity entity)
    {
        _entities.Remove(entity);
    }

    public void Update(TEntity entity)
    {
        _entities.Update(entity);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}