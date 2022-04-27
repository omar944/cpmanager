using API.Interfaces;
using AutoMapper.QueryableExtensions;
using Entities;

namespace API.Controllers;

public class CrudController<TInputDto, TOutputDto, TEntity> : BaseController where TEntity : BaseEntity
{
    private readonly IRepository<TEntity> _repository;
    private readonly IMapper _mapper;

    public CrudController(IRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TOutputDto>>> GetAll()
    {
        return await _repository.GetQuery().ProjectTo<TOutputDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] TInputDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        _repository.Add(entity);
        if (await _repository.SaveChangesAsync() == false)
            return BadRequest(new {message = "error creating resource"});
        return Ok(); //change to Created()
    }

    [HttpGet("{id:int}")]
    public virtual async Task<ActionResult<TOutputDto>> GetById(int id)
    {
        var result = await _repository.GetAsync(id);
        if (result == null) return NotFound();
        return _mapper.Map<TOutputDto>(result);
    }
}