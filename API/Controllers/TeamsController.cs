using API.DTOs;
using API.Extensions;
using API.Interfaces;
using AutoMapper.QueryableExtensions;
using Entities.App;

namespace API.Controllers;


public class TeamsController:BaseController
{
    private readonly IRepository<Team> _repository;
    private readonly IUserRepository _users;
    private readonly IMapper _mapper;

    public TeamsController(IRepository<Team> repository, IUserRepository users, IMapper mapper)
    {
        _repository = repository;
        _users = users;
        // _repository = new Repository<Team>(context);
        _mapper = mapper;
    }

    [HttpGet]
    public async Task< ActionResult<IEnumerable<TeamDto>> > GetUserTeams()
    {
        var id = User.GetUserId();
        var user = await _users.GetUserByIdAsync(id);
        var query = _repository.GetQuery().Include(x => x.Members)
            .Where(x => x.Members.Contains(user))
            .ProjectTo<TeamDto>(_mapper.ConfigurationProvider);
        return await query.ToListAsync();
    }
}