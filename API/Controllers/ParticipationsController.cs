using API.Interfaces;
using API.Models;
using Entities.App;

namespace API.Controllers;

public class ParticipationsController:CrudController<ParticipationCreateDto,ParticipationDto,Participation>
{
    public ParticipationsController(IRepository<Participation> repository, IMapper mapper, IUserRepository users) 
        :base(repository, mapper, users)
    {
    }
    
    [HttpPost]
    public override async Task<ActionResult> Create(ParticipationCreateDto dto)
    {
        var entity = Mapper.Map<Participation>(dto);
        entity.User = await GetUser();
        Repository.Add(entity);
        if (await Repository.SaveChangesAsync())
            return Created("",await Repository.GetProjectedById<ParticipationDto>(entity.Id));


        return BadRequest("failed creating object");
    }
}