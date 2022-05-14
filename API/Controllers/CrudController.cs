using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities;

namespace API.Controllers;

public abstract class CrudController<TInputDto, TOutputDto, TEntity> : BaseController
    where TEntity : BaseEntity
    where TOutputDto : BaseDto
{
    protected readonly IRepository<TEntity> Repository;
    protected readonly IMapper Mapper;

    protected CrudController(IRepository<TEntity> repository, IMapper mapper,IUserRepository users):base(users)
    {
        Repository = repository;
        Mapper = mapper;
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TOutputDto>>> GetAll()
    {
        var result = await Repository.GetProjected<TOutputDto>();
        return Ok(result);
    }

    [HttpPost]
    public virtual async Task<ActionResult> Create([FromBody] TInputDto dto)
    {
        var entity = Mapper.Map<TEntity>(dto);
        Repository.Add(entity);
        if (await Repository.SaveChangesAsync() == false)
            return BadRequest(new {message = "error creating resource"});
        return Created("", Mapper.Map<TOutputDto>(await Repository.GetProjectedById<TOutputDto>(entity.Id)));
    }

    [HttpGet("{id:int}")]
    public virtual async Task<ActionResult<TOutputDto>> GetById(int id)
    {
        var result = await Repository.GetProjectedById<TOutputDto>(id);
        if (result == null) return NotFound();
        return Mapper.Map<TOutputDto>(result);
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> Delete(int id)
    {
        var entity = await Repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        Repository.Remove(entity);
        if (await Repository.SaveChangesAsync() == false) return BadRequest(new {message = "error deleting"});
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public virtual async Task<ActionResult> Update(int id, [FromBody] TInputDto dto)
    {
        var entity = await Repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        entity = Mapper.Map(dto,entity);
        Repository.Update(entity);
        if (await Repository.SaveChangesAsync() == false) return BadRequest(new {message = "error updating"});
        return Ok(Repository.GetProjectedById<TOutputDto>(id));
    }

}