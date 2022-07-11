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
    /// <summary>
    /// get teams of users with id equals @id
    /// </summary>
    /// <param name="id">id of the user to get teams</param>
    /// <returns></returns>
    [HttpGet("user-teams/{id:int}")]
    public async Task< ActionResult<IEnumerable<TeamDto>> > GetUserTeams(int id)
    {
        var user = await Users.GetUserByIdAsync(id);
        if (user is null) return BadRequest(new{message="no such user"});
        var query = Repository.GetQuery().Include(x => x.Members)
            .Where(x => x.Members!.Select(m=>m.User).Contains(user))
            .ProjectTo<TeamDto>(Mapper.ConfigurationProvider);
        return await query.ToListAsync();
    }
    
    [HttpPost]
    public override async Task<ActionResult> Create([FromBody] TeamCreateDto dto)
    {
        if (dto.Members.IsNullOrEmpty()) return BadRequest("you need to add at least on member");
        var members = await Users.GetUsersAsync(dto.Members!);
        var team = new Team
        {
            Name = dto.Name
        };
        var membersToAdd = members.Select(x => new TeamUser
        {
            User = x, Team = team
        });
        team.Members = membersToAdd.ToList();
        if (dto.CoachId is not null)
        {
            team.CoachId = dto.CoachId.Value;
        }
        Repository.Add(team);
        if (await Repository.SaveChangesAsync() == false)
            return BadRequest(new {message = "error creating resource"});

        return Created("",await Repository.GetProjectedById<TeamDto>(team.Id));
    }
    
    [HttpPut("{id:int}")]
    public override async Task<ActionResult> Update(int id, [FromBody] TeamCreateDto dto)
    {
        var team = await Repository.GetQuery().Include(x=>x.Members)
            .FirstOrDefaultAsync(x=>x.Id==id);
        if (team is null) return NotFound();
        team.Members?.Clear();
        var members = await Users.GetUsersAsync(dto.Members!);
        team.Members = members.Select(x => new TeamUser
        {
            User = x
        }).ToList();
        team.Name = dto.Name;
        Repository.Update(team);
        if (await Repository.SaveChangesAsync() == false) return BadRequest(new {message = "error updating"});
        var res =  await Repository.GetProjectedById<TeamDto>(team.Id);
        return Ok(res);
    }
    
    
}