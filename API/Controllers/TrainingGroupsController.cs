using API.Data;
using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.App;

namespace API.Controllers;

public class TrainingGroupsController:CrudController<TrainingGroupCreateDto,TrainingGroupDto,TrainingGroup>
{
    public TrainingGroupsController(IRepository<TrainingGroup> repository, IMapper mapper, IUserRepository users) :
        base(repository, mapper, users)
    {
    }
    
    
    [HttpPost]
    public override async Task<ActionResult> Create([FromBody] TrainingGroupCreateDto dto)
    {
        var user = await GetUser();
        var students = await Users.GetUsersAsync(dto.Students!);
        var group = new TrainingGroup
        {
            Coach = user,
            Name = dto.Name
        };
        group.Students = students.Select(st => new TrainingGroupUser
        {
            TrainingGroup=group,
            User=st
        }).ToList();
        
        Repository.Add(group);
        if (await Repository.SaveChangesAsync() == false)
            return BadRequest(new {message = "error creating group"});

        return Created("",Mapper.Map<TrainingGroupDto>(group));
    }
    

    [HttpPatch("{id:int}")]
    public override async Task<ActionResult> Update(int id, [FromBody]TrainingGroupCreateDto dto)
    {
        var group = await Repository.GetQuery().Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (group is null) return NotFound();
        
        var currentStudents = group.Students?.Select(x=>x.UserId).ToList();
        currentStudents?.AddRange(dto.Students!);
        currentStudents = currentStudents?.Distinct().ToList();
        
        var newStudents = await Users.GetUsersAsync(currentStudents!);
        group.Students = newStudents.Select(st => new TrainingGroupUser
        {
            TrainingGroup=group,
            User=st
        }).ToList();
        Repository.Update(group);
        if (await Repository.SaveChangesAsync() == false) return BadRequest(new {message = "error updating"});
        return Ok(await Repository.GetQuery().ProjectTo<TrainingGroupDto>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id));
    }
    
    [HttpDelete("{id:int}/students/{studentId:int}")]
    public async Task<ActionResult> DeleteMembers(int id,int studentId)
    {
        var group = await Repository.GetQuery().Include(x => x.Students).FirstOrDefaultAsync(x=>x.Id==id);
        if (group is null) return NotFound();

        var currentStudent = group.Students?.Where(x => x.UserId == studentId).SingleOrDefault();
        if (currentStudent is null) return NotFound();
        group.Students?.Remove(currentStudent);
        
        Repository.Update(group);
        if (await Repository.SaveChangesAsync() == false) return BadRequest(new {message = "error updating"});
        return Ok(await Repository.GetQuery().ProjectTo<TrainingGroupDto>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id));
    }
}