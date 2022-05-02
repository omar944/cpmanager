using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.App;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;


public class TeamsController:CrudController<TeamCreateDto,TeamDto,Team>
{

    public TeamsController(IRepository<Team> repository, IMapper mapper, IUserRepository users) :
        base(repository, mapper, users)
    {
        
    }

    [HttpGet("{username}")]
    public async Task< ActionResult<IEnumerable<TeamDto>> > GetUserTeams(string username)
    {
        var user = await Users.GetUserByUsernameAsync(username);
        if (user is null) return BadRequest(new{message="no such user"});
        var query = Repository.GetQuery().Include(x => x.Members)
            .Where(x => x.Members!.Contains(user))
            .ProjectTo<TeamDto>(Mapper.ConfigurationProvider);
        return await query.ToListAsync();
    }
    
    [HttpPost]
    public override async Task<ActionResult> Create([FromBody] TeamCreateDto dto)
    {
        if (dto.Members.IsNullOrEmpty()) return BadRequest();
        var members = await Users.GetUsersAsync(dto.Members!);
        var team = new Team
        {
            Name = dto.Name,
            Members = members
        };
        Repository.Add(team);
        if (await Repository.SaveChangesAsync() == false)
            return BadRequest(new {message = "error creating resource 23"});

        return Created("", Mapper.Map<TeamDto>(await Repository.GetByIdAsync(team.Id)));
    }
    
    [HttpPut("{id:int}")]
    public override async Task<ActionResult> Update(int id, [FromBody] TeamCreateDto dto)
    {
        var team = await Repository.GetQuery().Include(x=>x.Members)
            .FirstOrDefaultAsync(x=>x.Id==id);
        if (team is null) return NotFound();
        team.Members?.Clear();
        var members = await Users.GetUsersAsync(dto.Members!);
        team.Members = members;
        team.Name = dto.Name;
        Repository.Update(team);
        if (await Repository.SaveChangesAsync() == false) return BadRequest(new {message = "error updating"});
        var res = await Repository.GetQuery().ProjectTo<TeamDto>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id);
        return Ok(res);
    }
    
    
}